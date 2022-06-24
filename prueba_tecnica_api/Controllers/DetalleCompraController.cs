using Microsoft.AspNetCore.Mvc;
using prueba_tecnica_api.Models;
using System.Data;

namespace prueba_tecnica_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetalleCompraController : Controller
    {
        db dbop = new db();
        string msg = string.Empty;

        [HttpGet("{id}")]
        public List<DetalleCompra> Get(int id)
        {
            Compra comp = new Compra();
            comp.comp_id_compra = id;
            comp.type = "GETBYID";
            DataSet ds = dbop.DetalleCompraGetById(comp, out msg);
            List<DetalleCompra> list = new List<DetalleCompra>();

            bool hasRows = ds.Tables.Cast<DataTable>()
                .Any(table => table.Rows.Count != 0);
            if (hasRows)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new DetalleCompra
                    {
                        detc_id_detalle_compra = Convert.ToInt32(dr["DETC_ID_DETALLE_COMPRA"]),
                        detc_nombre_producto = Convert.ToString(dr["DETC_NOMBRE_PRODUCTO"]),
                        detc_cantidad_producto = Convert.ToInt32(dr["DETC_CANTIDAD_PRODUCTO"]),
                        detc_importe = Convert.ToInt32(dr["DETC_IMPORTE"]),
                        detc_precio_producto = Convert.ToDouble(dr["DETC_PRECIO_PRODUCTO"]),
                        detc_comp_id_compra = Convert.ToInt32(dr["DETC_COMP_ID_COMPRA"])
                    });
                }
            }
            return list;
        }
        
    }
}
