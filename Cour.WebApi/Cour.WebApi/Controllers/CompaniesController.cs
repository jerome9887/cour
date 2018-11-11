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
    [RoutePrefix("Companies")]
    public class CompaniesController : ApiController
    {
        private CourContext db = new CourContext();

        [Route("GetAll")]
        [HttpGet]
        public IEnumerable<Company> GetAll()
        {
            return db.Companies.ToList();
        }

        [Route("Get")]
        [HttpGet]
        public Company Get(Guid id)
        {
            return db.Companies.FirstOrDefault(x => x.Id == id);
        }

        [Route("Add")]
        [HttpPost]
        public void Add([FromBody] Company value)
        {
            db.Companies.Add(value);
            db.SaveChanges();
        }

        [Route("AddMany")]
        [HttpPost]
        public void AddMany([FromBody] IEnumerable<Company> value)
        {
            foreach (var v in value)
            {
                db.Companies.Add(v);
            }
            db.SaveChanges();
        }


        [Route("Update")]
        [HttpPut]
        public void Update([FromBody] Company value)
        {
            var v = db.Companies.FirstOrDefault(x => x.Id == value.Id);
            db.Companies.Remove(v);
            db.Companies.Add(value);
            db.SaveChanges();
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            db.Companies.Remove(db.Companies.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
        }
    }
}