namespace prueba_tecnica_api.Models
{
    public class DetalleCompra
    {
        public int detc_id_detalle_compra { get; set; }

        public string? detc_nombre_producto { get; set; } = "";
        public double? detc_precio_producto { get; set; } = 0;
        public int detc_cantidad_producto { get; set; } = 0;
        public double detc_importe { get; set; } = 0;
        public int detc_comp_id_compra{ get; set; }
    }
}
