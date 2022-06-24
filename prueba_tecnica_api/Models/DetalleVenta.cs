namespace prueba_tecnica_api.Models
{
    public class DetalleVenta
    {
        public int detv_id_venta { get; set; }
        public string? detv_nombre_producto { get; set; }
        public double? detv_precio_producto { get; set; }
        public int? detv_cantidad_producto { get; set; }
        public double detv_importe { get; set; } = 0;
        public int detv_ven_id_venta { get; set; }


    }
}
