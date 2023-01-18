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
    public class PessoasController : ApiController
    {
        private ListaContatosContext db = new ListaContatosContext();

        // GET: api/Pessoas
        [HttpGet]
        public IQueryable<PessoasModel> GetPessoas()
        {
            return db.tabPessoas;
        }

        // GET: api/Pessoas/5
        [HttpGet]
        [ResponseType(typeof(PessoasModel))]
        public IHttpActionResult GetPessoa(int id)
        {
            PessoasModel pessoasModel = db.tabPessoas.Find(id);
            if (pessoasModel == null)
            {
                return NotFound();
            }

            return Ok(pessoasModel);
        }

        // PUT: api/Pessoas/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPessoa(int id, PessoasModel pessoasModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pessoasModel.id)
            {
                return BadRequest();
            }

            db.Entry(pessoasModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PessoasModelExists(id))
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

        // POST: api/Pessoas
        [HttpPost]
        [ResponseType(typeof(PessoasModel))]
        public IHttpActionResult PostPessoa(PessoasModel pessoasModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tabPessoas.Add(pessoasModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = pessoasModel.id }, pessoasModel);
        }

        // DELETE: api/Pessoas/5
        [HttpDelete]
        [ResponseType(typeof(PessoasModel))]
        public IHttpActionResult DeletePessoa(int id)
        {
            PessoasModel pessoasModel = db.tabPessoas.Find(id);
            if (pessoasModel == null)
                return NotFound();

            List<ContatosModel> contatosModel = db.tabContatos.Where(contatos => contatos.id_pessoa == id).ToList();

            foreach (var contatos in contatosModel)
            {
                IQueryable<TelefonesModel> telefonesModel = db.tabTelefones.Where(telefones => telefones.id_contato == contatos.id);
                foreach (var tel in telefonesModel)
                    db.tabTelefones.Remove(tel);

                IQueryable<EmailsModel> emailModel = db.tabEmails.Where(email => email.id_contato == contatos.id);
                foreach (var email in emailModel)
                    db.tabEmails.Remove(email);

                db.SaveChanges();

                db.tabContatos.Remove(contatos);
                db.SaveChanges();
            }

            db.tabPessoas.Remove(pessoasModel);
            db.SaveChanges();

            return Ok(pessoasModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PessoasModelExists(int id)
        {
            return db.tabPessoas.Count(e => e.id == id) > 0;
        }
    }
}