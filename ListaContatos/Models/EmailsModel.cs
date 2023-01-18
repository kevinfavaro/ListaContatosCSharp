using System.ComponentModel.DataAnnotations.Schema;

namespace ListaContatos.Models
{
    [Table("tab_emails")]
    public class EmailsModel
    {
        public int id { get; set; }
        public int id_contato { get; set; }
        public string email { get; set; }
    }
}