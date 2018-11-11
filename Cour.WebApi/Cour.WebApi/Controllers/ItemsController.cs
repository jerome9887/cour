using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Cour.WebApi.Models;

namespace Ludrina.WebApi.Controllers
{
    [RoutePrefix("Items")]
    public class ItemsController : ApiController
    {
        private CourContext db = new CourContext();

        [Route("GetAll")]
        [HttpGet]
        public IEnumerable<Item> GetAll()
        {
            return db.Items.ToList();
        }

        [Route("Get")]
        [HttpGet]
        public Item Get(Guid id)
        {
            return db.Items.FirstOrDefault(x => x.Id == id);
        }

        [Route("Add")]
        [HttpPost]
        public void Add([FromBody] Item value)
        {
            db.Items.Add(value);
            db.SaveChanges();
        }

        [Route("AddMany")]
        [HttpPost]
        public void AddMany([FromBody] IEnumerable<Item> value)
        {
            foreach (var v in value)
            {
                db.Items.Add(v);
            }
            db.SaveChanges();
        }


        [Route("Update")]
        [HttpPut]
        public void Update([FromBody] Item value)
        {
            var v = db.Items.FirstOrDefault(x => x.Id == value.Id);
            db.Items.Remove(v);
            db.Items.Add(value);
            db.SaveChanges();
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            db.Items.Remove(db.Items.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
        }
    }
}