using System.ComponentModel.DataAnnotations.Schema;

namespace ListaContatos.Models
{
    [Table("tab_contatos")]
    public class ContatosModel
    {
        public int id { get; set; }
        public int id_pessoa { get; set; }
        public string nome { get; set; }

    }
}