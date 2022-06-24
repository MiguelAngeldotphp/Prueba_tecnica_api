namespace prueba_tecnica_api.filters
{
    public class Mensaje_Error
    {
        private string mensaje;

        public Mensaje_Error(string mensaje)
        {
            this.mensaje = mensaje;
        }
        public Mensaje_Error()
        {
        }
        public string Mensaje { get => mensaje; set => mensaje = value; }
    }
}
