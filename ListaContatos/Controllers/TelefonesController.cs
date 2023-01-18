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
    public class TelefonesController : ApiController
    {
        private ListaContatosContext db = new ListaContatosContext();

        // GET: api/Telefones
        [HttpGet]
        public IQueryable<TelefonesModel> GetTelefones()
        {
            return db.tabTelefones;
        }

        // GET: api/Telefones/5
        [HttpGet]
        [ResponseType(typeof(TelefonesModel))]
        public IHttpActionResult GetTelefone(int id)
        {
            TelefonesModel telefonesModel = db.tabTelefones.Find(id);
            if (telefonesModel == null)
            {
                return NotFound();
            }

            return Ok(telefonesModel);
        }

        [HttpGet]
        [ResponseType(typeof(TelefonesModel))]
        public IQueryable<TelefonesModel> GetTelefonePorContato(int id_contato)
        {
            return db.tabTelefones.Where(telefone => telefone.id_contato == id_contato);
        }

        [HttpGet]
        [ResponseType(typeof(TelefonesModel))]
        public IQueryable<TelefonesModel> GetTelefonePorPessoa(int id_pessoa)
        {
            return from t in db.tabTelefones
                   join c in db.tabContatos on t.id_contato equals c.id
                   where c.id_pessoa == id_pessoa
                   select t;
        }

        // PUT: api/Telefones/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTelefone(int id, TelefonesModel telefonesModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != telefonesModel.id)
            {
                return BadRequest();
            }

            db.Entry(telefonesModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelefonesModelExists(id))
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

        // POST: api/Telefones
        [HttpPost]
        [ResponseType(typeof(TelefonesModel))]
        public IHttpActionResult PostTelefone(TelefonesModel telefonesModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tabTelefones.Add(telefonesModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = telefonesModel.id }, telefonesModel);
        }

        // DELETE: api/Telefones/5
        [HttpDelete]
        [ResponseType(typeof(TelefonesModel))]
        public IHttpActionResult DeleteTelefone(int id)
        {
            TelefonesModel telefonesModel = db.tabTelefones.Find(id);
            if (telefonesModel == null)
            {
                return NotFound();
            }

            db.tabTelefones.Remove(telefonesModel);
            db.SaveChanges();

            return Ok(telefonesModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TelefonesModelExists(int id)
        {
            return db.tabTelefones.Count(e => e.id == id) > 0;
        }
    }
}