using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using ListaContatos.Data;
using ListaContatos.Models;

namespace ListaContatos.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ContatosController : ApiController
    {
        private ListaContatosContext db = new ListaContatosContext();

        // GET: api/Contatos
        public IQueryable<ContatosModel> GetContatos()
        {
            return db.tabContatos;
        }

        // GET: api/Contatos/5
        [ResponseType(typeof(ContatosModel))]
        public IHttpActionResult GetContato(int id)
        {
            ContatosModel contatosModel = db.tabContatos.Find(id);
            if (contatosModel == null)
            {
                return NotFound();
            }

            return Ok(contatosModel);
        }

        [ResponseType(typeof(ContatosModel))]
        public IQueryable<ContatosModel> GetContatosPorPessoa(int id_pessoa)
        {
            return db.tabContatos.Where(contato => contato.id_pessoa == id_pessoa);
        }

        // PUT: api/Contatos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContato(int id, ContatosModel contatosModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contatosModel.id)
            {
                return BadRequest();
            }

            db.Entry(contatosModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContatosModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Contatos
        [ResponseType(typeof(ContatosModel))]
        public IHttpActionResult PostContato(ContatosModel contatosModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tabContatos.Add(contatosModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = contatosModel.id }, contatosModel);
        }

        // DELETE: api/Contatos/5
        [ResponseType(typeof(ContatosModel))]
        public IHttpActionResult DeleteContato(int id)
        {
            ContatosModel contatosModel = db.tabContatos.Find(id);
            if (contatosModel == null)
                return NotFound();

            IQueryable<TelefonesModel> telefonesModel = db.tabTelefones.Where(telefones => telefones.id_contato == id);
            foreach (var tel in telefonesModel)
                db.tabTelefones.Remove(tel);

            IQueryable<EmailsModel> emailModel = db.tabEmails.Where(email => email.id_contato == id);
            foreach (var email in emailModel)
                db.tabEmails.Remove(email);

            db.SaveChanges();
            db.tabContatos.Remove(contatosModel);
            db.SaveChanges();

            return Ok(contatosModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContatosModelExists(int id)
        {
            return db.tabContatos.Count(e => e.id == id) > 0;
        }
    }
}