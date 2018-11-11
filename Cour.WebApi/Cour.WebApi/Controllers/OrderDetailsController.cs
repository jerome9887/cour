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
    [RoutePrefix("OrderDetails")]
    public class OrderDetailsController : ApiController
    {
        private CourContext db = new CourContext();

        [Route("GetAll")]
        [HttpGet]
        public IEnumerable<OrderDetail> GetAll()
        {
            return db.OrderDetails.ToList();
        }

        [Route("GetAllByOrderId")]
        [HttpGet]
        public IEnumerable<OrderDetail> GetAllByOrderId(Guid id)
        {
            return db.OrderDetails.Where(x => x.OrderId == id).ToList();
        }

        [Route("Get")]
        [HttpGet]
        public OrderDetail Get(Guid id)
        {
            return db.OrderDetails.FirstOrDefault(x => x.Id == id);
        }

        [Route("Add")]
        [HttpPost]
        public void Add([FromBody] OrderDetail value)
        {
            db.OrderDetails.Add(value);
            db.SaveChanges();
        }

        [Route("AddMany")]
        [HttpPost]
        public void AddMany([FromBody] IEnumerable<OrderDetail> value)
        {
            foreach (var v in value)
            {
                db.OrderDetails.Add(v);
            }
            db.SaveChanges();
        }


        [Route("Update")]
        [HttpPut]
        public void Update([FromBody] OrderDetail value)
        {
            var v = db.OrderDetails.FirstOrDefault(x => x.Id == value.Id);
            db.OrderDetails.Remove(v);
            db.OrderDetails.Add(value);
            db.SaveChanges();
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            db.OrderDetails.Remove(db.OrderDetails.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
        }
    }
}