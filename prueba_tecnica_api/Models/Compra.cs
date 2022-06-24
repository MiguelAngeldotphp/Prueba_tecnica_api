using System.Diagnostics;

namespace prueba_tecnica_api.Models
{
    public class Compra
    {
        public int comp_id_compra { get; set; }
        public double comp_monto_total { get; set; }
        public string? comp_fecha_registro { get; set; }
        public string? comp_proveedor { get; set; }
        public string? comp_usuario { get; set; } 
        public string? type { get; set; }
        public DetalleCompra[]? detalles { get; set; } = { };


        public bool verificarMontoTotal()
        {
            double monto_total_detalles = 0;

            for (int i = 0; i < detalles?.Length; i++)
            {
                monto_total_detalles += detalles[i].detc_importe;

            }
            Debug.WriteLine("monto detalles: " + monto_total_detalles);
            Debug.WriteLine("monto_compra: " + this.comp_monto_total);

            if (monto_total_detalles != this.comp_monto_total)
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
