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
    [RoutePrefix("Users")]
    public class UsersController : ApiController
    {
        private CourContext db = new CourContext();

        [Route("GetAll")]
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return db.Users.ToList();
        }

        [Route("Get")]
        [HttpGet]
        public User Get(Guid id)
        {
            return db.Users.FirstOrDefault(x => x.Id == id);
        }

        [Route("GetUserLogin")]
        [HttpGet]
        public User GetUserLogin(string username, string password)
        {
            return db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        [Route("Add")]
        [HttpPost]
        public void Add([FromBody] User value)
        {
            value.Id = Guid.NewGuid();

            db.Users.Add(value);
            db.SaveChanges();
        }

        [Route("AddMany")]
        [HttpPost]
        public void AddMany([FromBody] IEnumerable<User> value)
        {
            foreach (var v in value)
            {
                db.Users.Add(v);
            }
            db.SaveChanges();
        }


        [Route("Update")]
        [HttpPut]
        public void Update([FromBody] User value)
        {
            var v = db.Users.FirstOrDefault(x => x.Id == value.Id);
            db.Users.Remove(v);
            db.Users.Add(value);
            db.SaveChanges();
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            db.Users.Remove(db.Users.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
        }
    }
}