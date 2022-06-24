using Microsoft.AspNetCore.Mvc;
using prueba_tecnica_api.filters;
using prueba_tecnica_api.Models;
using System.Data;

namespace prueba_tecnica_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        db dbop = new db();
        string msg = string.Empty;

        [HttpGet]
        public List<Venta> Get()
        {
            Venta vent = new Venta();
            vent.type = "GET";
            DataSet ds = dbop.VentaGet(vent, out msg);
            List<Venta> list = new List<Venta>();
            bool hasRows = ds.Tables.Cast<DataTable>()
               .Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Venta
                    {
                        ven_id_venta = Convert.ToInt32(dr["VEN_ID_VENTA"]),
                        ven_fecha_registro = Convert.ToDateTime(dr["VEN_FECHA_REGISTRO"]),
                        ven_monto_total = Convert.ToDouble(dr["VEN_MONTO_TOTAL"]),
                        ven_cliente = dr["VEN_CLIENTE"].ToString(),
                        ven_usuario = Convert.ToString(dr["VEN_USUARIO"])

                    });
                }
            }
            return list;
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] Venta vent)
        {
            Mensaje_Error mensaje_error = new Mensaje_Error();
            string msg = string.Empty;
            bool exito = false;
            try
            {
                if (vent.detalles?.Length == 0)
                {
                    mensaje_error = new Mensaje_Error("La venta no ha podido ser realizada. Envie el detalle de venta.");
                    return BadRequest(mensaje_error);
                }
                else
                {
                    if (!vent.verificarMontoTotal())
                    {
                        mensaje_error = new Mensaje_Error("La suma de los detalles no coincide con el monto total de la venta.");
                        return BadRequest(mensaje_error);
                    }
                    else
                    {
                        vent.ven_cliente = vent.ven_cliente?.ToUpper();
                        vent.ven_usuario = vent.ven_usuario?.ToUpper();
                        vent.type = "INSERT";

                        for (int i = 0; i < vent.detalles?.Length; i++)
                        {
                            vent.detalles[i].detv_nombre_producto = vent.detalles[i].detv_nombre_producto?.ToUpper();
                        }

                        exito = dbop.VentaPost(vent);
                        if (exito)
                        {
                            msg = "La Venta se realizó correctamente";
                            return Ok(msg);
                        }
                        else
                        {
                            mensaje_error = new Mensaje_Error("Error al realizar la venta");
                            return BadRequest(mensaje_error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return BadRequest(msg);
            }

        }
    }
}
