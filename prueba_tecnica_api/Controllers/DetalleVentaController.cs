using Microsoft.AspNetCore.Mvc;
using prueba_tecnica_api.Models;
using System.Data;

namespace prueba_tecnica_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleVentaController : ControllerBase
    {
        db dbop = new db();
        string msg = string.Empty;

        [HttpGet("{id}")]
        public List<DetalleVenta> Get(int id)
        {
            Venta vent = new Venta();
            vent.ven_id_venta = id;
            vent.type = "GETBYID";
            DataSet ds = dbop.DetalleVentaGetById(vent, out msg);
            List<DetalleVenta> list = new List<DetalleVenta>();
            bool hasRows = ds.Tables.Cast<DataTable>()
                .Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new DetalleVenta
                    {
                        detv_ven_id_venta = Convert.ToInt32(dr["DETV_ID_DETALLE_VENTA"]),
                        detv_nombre_producto = Convert.ToString(dr["DETV_NOMBRE_PRODUCTO"]),
                        detv_cantidad_producto = Convert.ToInt32(dr["DETV_CANTIDAD_PRODUCTO"]),
                        detv_importe = Convert.ToInt32(dr["DETV_IMPORTE"]),
                        detv_precio_producto = Convert.ToDouble(dr["DETV_PRECIO_PRODUCTO"]),
                        detv_id_venta = Convert.ToInt32(dr["DETV_VEN_ID_VENTA"])
                    });
                }
            }

            return list;
        }
    }
}
