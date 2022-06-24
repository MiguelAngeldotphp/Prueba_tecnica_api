using Microsoft.AspNetCore.Mvc;
using prueba_tecnica_api.Models;
using System.Data;
using prueba_tecnica_api.filters;
using System.Web;
using System.Diagnostics;

namespace prueba_tecnica_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class CompraController : ControllerBase
    {
        db dbop = new db();
        string msg = string.Empty;

        [HttpGet]
        public List<Compra> Get()
        {
            Compra comp = new Compra();
            comp.type = "GET";
           
            DataSet ds = dbop.CompraGet(comp, out msg);
            List<Compra> list = new List<Compra>();
            bool hasRows = ds.Tables.Cast<DataTable>()
                .Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new Compra
                    {
                        comp_id_compra = Convert.ToInt32(dr["COMP_ID_COMPRA"]),
                        comp_fecha_registro = Convert.ToString(dr["COMP_FECHA_REGISTRO"]),
                        comp_monto_total = Convert.ToDouble(dr["COMP_MONTO_TOTAL"]),
                        comp_proveedor = dr["COMP_PROVEEDOR"].ToString(),
                        comp_usuario = Convert.ToString(dr["COMP_USUARIO"])

                    });
                }

            }

            return list; 
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] Compra comp)
        {
            Mensaje_Error mensaje_error = new Mensaje_Error();
            string msg = string.Empty;
            bool exito = false;
            try
            {
                if (comp.detalles?.Length == 0)
                {
                    mensaje_error = new Mensaje_Error("La compra no ha podido ser realizada. Envie el detalle de compra.");
                    return BadRequest(mensaje_error);
                }
                else
                {
                    if (!comp.verificarMontoTotal())
                    {
                        mensaje_error = new Mensaje_Error("La suma de los detalles no coincide con el monto total de la compra.");
                        return BadRequest(mensaje_error);
                    }
                    else
                    {
                        comp.comp_proveedor = comp.comp_proveedor?.ToUpper();
                        comp.comp_usuario = comp.comp_usuario?.ToUpper();
                        comp.type = "INSERT"; 

                        for (int i = 0; i < comp.detalles?.Length; i++)
                        {
                            comp.detalles[i].detc_nombre_producto = comp.detalles[i].detc_nombre_producto?.ToUpper();
                        }

                        exito = dbop.CompraPost(comp);
                        if (exito){
                            msg = "La compra se realizó correctamente";
                            return Ok();
                        }
                        else
                        {
                            mensaje_error = new Mensaje_Error("Error al realizar la compra");
                            return BadRequest(mensaje_error);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
                return BadRequest(msg);
            }

        }
    }
}
