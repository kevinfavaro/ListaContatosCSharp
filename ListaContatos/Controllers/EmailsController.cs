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
    public class EmailsController : ApiController
    {
        private ListaContatosContext db = new ListaContatosContext();

        // GET: api/Emails
        public IQueryable<EmailsModel> GetEmails()
        {
            return db.tabEmails;
        }

        // GET: api/Emails/5
        [ResponseType(typeof(EmailsModel))]
        public IHttpActionResult GetEmail(int id)
        {
            EmailsModel emailsModel = db.tabEmails.Find(id);
            if (emailsModel == null)
            {
                return NotFound();
            }

            return Ok(emailsModel);
        }

        [HttpGet]
        [ResponseType(typeof(EmailsModel))]
        public IQueryable<EmailsModel> GetTelefonePorContato(int id_contato)
        {
            return db.tabEmails.Where(telefone => telefone.id_contato == id_contato);
        }

        [HttpGet]
        [ResponseType(typeof(EmailsModel))]
        public IQueryable<EmailsModel> GetTelefonePorPessoa(int id_pessoa)
        {
            return from t in db.tabEmails
                   join c in db.tabContatos on t.id_contato equals c.id
                   where c.id_pessoa == id_pessoa
                   select t;
        }

        // PUT: api/Emails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmail(int id, EmailsModel emailsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emailsModel.id)
            {
                return BadRequest();
            }

            db.Entry(emailsModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailsModelExists(id))
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

        // POST: api/Emails
        [ResponseType(typeof(EmailsModel))]
        public IHttpActionResult PostEmail(EmailsModel emailsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tabEmails.Add(emailsModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = emailsModel.id }, emailsModel);
        }

        // DELETE: api/Emails/5
        [ResponseType(typeof(EmailsModel))]
        public IHttpActionResult DeleteEmail(int id)
        {
            EmailsModel emailsModel = db.tabEmails.Find(id);
            if (emailsModel == null)
            {
                return NotFound();
            }

            db.tabEmails.Remove(emailsModel);
            db.SaveChanges();

            return Ok(emailsModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmailsModelExists(int id)
        {
            return db.tabEmails.Count(e => e.id == id) > 0;
        }
    }
}