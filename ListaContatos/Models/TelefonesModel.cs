using System.ComponentModel.DataAnnotations.Schema;

namespace ListaContatos.Models
{
    [Table("tab_telefones")]
    public class TelefonesModel
    {
        public int id { get; set; }
        public int id_contato { get; set; }
        public int pais { get; set; }
        public int DDD { get; set; }
        public int numero { get; set; }
        public bool whatsapp { get; set; }
        public string observacao { get; set; }
    }
}