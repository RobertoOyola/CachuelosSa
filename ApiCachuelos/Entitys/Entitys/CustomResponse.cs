namespace Entitys.Entitys
{
    public class CustomResponse<T>
    {
        public CustomHeader Header { get; set; }
        public T Body { get; set; }
    }

    public class CustomHeader
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
    }

    public class ServiceResult<T>
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public int Codigo { get; set; }
        public T? Datos { get; set; }

        public static ServiceResult<T> Ok(T datos, string mensaje, int codigo) =>
            new() { Exitoso = true, Datos = datos, Mensaje = mensaje, Codigo = codigo };

        public static ServiceResult<T> Fail(string mensaje, int codigo) =>
            new() { Exitoso = false, Datos = default, Mensaje = mensaje, Codigo = codigo };
    }
}
