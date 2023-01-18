using System.ComponentModel.DataAnnotations.Schema;

namespace ListaContatos.Models
{
    [Table("tab_pessoas")]
    public class PessoasModel
    {
        public int id { get; set; }
        public string nome { get; set; }
    }
}