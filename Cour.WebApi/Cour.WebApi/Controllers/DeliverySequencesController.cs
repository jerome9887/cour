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
    [RoutePrefix("DeliverySequences")]
    public class DeliverySequencesController : ApiController
    {
        private CourContext db = new CourContext();

        [Route("GetAll")]
        [HttpGet]
        public IEnumerable<DeliverySequence> GetAll()
        {
            return db.DeliverySequences.ToList();
        }

        [Route("GetAllByOrderId")]
        [HttpGet]
        public IEnumerable<DeliverySequence> GetAllByOrderId(Guid id)
        {
            return db.DeliverySequences.Where(x => x.Status != "..." && x.OrderId == id).GroupBy(x => x.OrderId)
                            .SelectMany(g => g.OrderBy(x => x.Sequence)).ToList();
        }

        [Route("Get")]
        [HttpGet]
        public DeliverySequence Get(Guid id)
        {
            return db.DeliverySequences.FirstOrDefault(x => x.Id == id);
        }

        [Route("GetByOrderIdAndUserId")]
        [HttpGet]
        public DeliverySequence GetByOrderIdAndUserId(Guid orderId, Guid userId)
        {
            return db.DeliverySequences.FirstOrDefault(x => x.OrderId == orderId && x.UserId == userId);
        }


        [Route("GetAllByOrderIdAndUserId")]
        [HttpGet]
        public IEnumerable<DeliverySequence> GetAllByOrderIdAndUserId(Guid orderId, Guid userId)
        {
            return db.DeliverySequences.Where(x => x.OrderId == orderId && x.UserId == userId);
        }

        [Route("Add")]
        [HttpPost]
        public void Add([FromBody] DeliverySequence value)
        {
            db.DeliverySequences.Add(value);
            db.SaveChanges();
        }

        [Route("AddMany")]
        [HttpPost]
        public void AddMany([FromBody] IEnumerable<DeliverySequence> value)
        {
            foreach (var v in value)
            {
                db.DeliverySequences.Add(v);
            }
            db.SaveChanges();
        }


        [Route("UpdateReceivedDate")]
        [HttpPut]
        public void UpdateReceivedDate([FromBody] DeliverySequence value)
        {
            var v = db.DeliverySequences.FirstOrDefault(x => x.Id == value.Id);
            value.DateReceived = DateTime.Now;
            value.Status = "Received";
            db.DeliverySequences.Remove(v);
            db.DeliverySequences.Add(value);

            var o = db.Orders.FirstOrDefault(x => x.Id == v.OrderId);
            var user = db.Users.FirstOrDefault(x => x.Id == value.UserId);
            var port = db.Ports.FirstOrDefault(x => x.Id == user.PortId);
            o.Status = "On Going: Received by " + port.Name;

            db.SaveChanges();
        }


        [Route("UpdateTransferedDate")]
        [HttpPut]
        public void UpdateTransferdDate([FromBody] DeliverySequence value)
        {
            var v = db.DeliverySequences.FirstOrDefault(x => x.Id == value.Id);
            value.DateTransfered = DateTime.Now;
            value.Status = "Transfered";

            List<DeliverySequence> deliverySequences =
                db.DeliverySequences.Where(x => x.Status == "..." && x.OrderId == value.OrderId).OrderBy(x => x.Sequence).ToList();

            if(deliverySequences != null)
            {
                var nv = deliverySequences[0];
                nv.Status = "Pending";
                var o = db.Orders.FirstOrDefault(x => x.Id == v.OrderId);
                var user = db.Users.FirstOrDefault(x => x.PortId == nv.UserId);
                var port = db.Ports.FirstOrDefault(x => x.Id == user.PortId);
                o.Status = "On Going: Transfered to " + port.Name;
            }
            else
            {
                var nv = deliverySequences[0];
                nv.Status = "Pending";
                var o = db.Orders.FirstOrDefault(x => x.Id == v.OrderId);
                o.Status = "COMPLETED";
            }
   


            db.DeliverySequences.Remove(v);
            db.DeliverySequences.Add(value);

            db.SaveChanges();
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            db.DeliverySequences.Remove(db.DeliverySequences.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
        }
    }
}