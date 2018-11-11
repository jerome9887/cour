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
    [RoutePrefix("Ports")]
    public class PortsController : ApiController
    {
        private CourContext db = new CourContext();

        [Route("GetAll")]
        [HttpGet]
        public IEnumerable<Port> GetAll()
        {
            return db.Ports.ToList();
        }

        [Route("GetAllPortsByCompany")]
        [HttpGet]
        public IEnumerable<Port> GetAllPortsByCompany(string company)
        {
            Company com = db.Companies.FirstOrDefault(x => x.Name == company);
            List<User> users = db.Users.Where(x => x.CompanyId == com.Id).ToList();
            List<Port> ports = new List<Port>();
            foreach(User user in users)
            {
                Port port = db.Ports.FirstOrDefault(x => x.Id == user.PortId);
                ports.Add(port);
            }
            return ports;
        }

        [Route("GetAllPortsByCompany")]
        [HttpGet]
        public IEnumerable<Port> GetAllPortsByCompany()
        {
            return db.Ports.Where(x => 
            x.Name != "Production Planning" &&
            x.Name != "Raw Materials Issuance" &&
            x.Name != "Production" &&
            x.Name != "Finished Goods Receiving" &&
            x.Name != "Transport Planning" &&
            x.Name != "Loading");
        }

        [Route("Get")]
        [HttpGet]
        public Port Get(Guid id)
        {
            return db.Ports.FirstOrDefault(x => x.Id == id);
        }

        [Route("GetByUserId")]
        [HttpGet]
        public Port GetByUserId(Guid id)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == id);
            return db.Ports.FirstOrDefault(x => x.Id == user.PortId);
        }
        [Route("Add")]
        [HttpPost]
        public void Add([FromBody] Port value)
        {
            db.Ports.Add(value);
            db.SaveChanges();
        }

        [Route("AddMany")]
        [HttpPost]
        public void AddMany([FromBody] IEnumerable<Port> value)
        {
            foreach (var v in value)
            {
                db.Ports.Add(v);
            }
            db.SaveChanges();
        }


        [Route("Update")]
        [HttpPut]
        public void Update([FromBody] Port value)
        {
            var v = db.Ports.FirstOrDefault(x => x.Id == value.Id);
            db.Ports.Remove(v);
            db.Ports.Add(value);
            db.SaveChanges();
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            db.Ports.Remove(db.Ports.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
        }
    }
}