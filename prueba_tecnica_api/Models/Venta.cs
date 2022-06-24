namespace prueba_tecnica_api.Models
{
    public class Venta
    {
        public int ven_id_venta { get; set; }
        public double ven_monto_total{ get; set; }
        public System.Nullable<DateTime> ven_fecha_registro { get; set; }
        public string? ven_cliente { get; set; }
        public string? ven_usuario{ get; set; }
        public string?  type { get; set; }
        public DetalleVenta[]? detalles { get; set; }

        public bool verificarMontoTotal()
        {
            double monto_total_detalles = 0;

            for (int i = 0; i < detalles?.Length; i++)
            {
                monto_total_detalles += detalles[i].detv_importe;

            }
            if (monto_total_detalles != this.ven_monto_total)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}
