
namespace Entitys.Entitys.Documentos
{
    public class Documentos
    {
        public string UrlString { get; set; }
        public int idTipoDocumento { get; set; }
        
    }

    public class DocResponse
    {
        public int Id { get; set; }

        public string UrlDocumento { get; set; }

        public int IdTipoDocumentos { get; set; }

        public bool? Activo { get; set; }
    }

    public class ListDocumentos
    {
        public Docus Curriculum { get; set; }
        public Docus HistorialPolicial { get; set; }
        public List<Docus> Titulos { get; set; }
    }

    public class Docus
    {
        public int Id { get; set; }

        public string UrlDocumento { get; set; }
    }

    public class TipDoc
    {
        public int Id { get; set; }
        public string TituloDoc {  get; set; }
    }
}
