using ListaContatos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ListaContatos.Data
{
    public class ListaContatosContext : DbContext
    {
        public ListaContatosContext() : base("name=ListaContatos")
        {
        }

        public DbSet<PessoasModel> tabPessoas { get; set; }

        public DbSet<ContatosModel> tabContatos { get; set; }

        public DbSet<TelefonesModel> tabTelefones { get; set; }

        public DbSet<EmailsModel> tabEmails { get; set; }
    }
}
