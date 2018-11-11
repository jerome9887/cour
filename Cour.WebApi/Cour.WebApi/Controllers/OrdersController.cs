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
    [RoutePrefix("Orders")]
    public class OrdersController : ApiController
    {
        private CourContext db = new CourContext();

        [Route("GetAllByUserId")]
        [HttpGet]
        public IEnumerable<Order> GetAllByUserId(Guid userId, string phase, string category)
        {
            User user = db.Users.FirstOrDefault(x => x.Id == userId);
            Company company = db.Companies.FirstOrDefault(x => x.Id == user.CompanyId);
            List<DeliverySequence> deliverySequences = new List<DeliverySequence>();

            if (user.Username != "admin" && company.Name != "PilMico")
            {
                if (category == "All")
                {
                    if (phase == "All")
                    {
                        deliverySequences = db.DeliverySequences.Where(x => x.Status != "..." && x.Sequence > 6).GroupBy(x => x.OrderId)
                            .SelectMany(g => g.OrderByDescending(x => x.Sequence).Take(1)).ToList();
                    }
                    else
                    {
                        Port port = db.Ports.FirstOrDefault(x => x.Name == phase);
                        User u = db.Users.FirstOrDefault(x => x.PortId == port.Id);
                        deliverySequences = db.DeliverySequences.Where(x => x.UserId == u.Id && x.Status != "..." && x.Sequence > 6).GroupBy(x => x.OrderId)
                          .SelectMany(g => g.OrderByDescending(x => x.Sequence).Take(1)).ToList();
                    }
                }
                else
                {
                    if (phase == "All")
                    {
                        deliverySequences = db.DeliverySequences.Where(x => x.Status != "..." && x.Sequence > 6).GroupBy(x => x.OrderId)
                          .SelectMany(g => g.OrderByDescending(x => x.Sequence).Take(1).Where(x => x.Status == category)).ToList();
                    }
                    else
                    {
                        Port port = db.Ports.FirstOrDefault(x => x.Name == phase);
                        User u = db.Users.FirstOrDefault(x => x.PortId == port.Id);
                        deliverySequences = db.DeliverySequences.Where(x => x.Status == category && x.UserId == u.Id && x.Sequence > 6).GroupBy(x => x.OrderId)
                         .SelectMany(g => g.OrderByDescending(x => x.Sequence).Take(1)).ToList();
                    }

                }
            }
            else if(user.UserType == "Admin")
            {
                if(category == "All")
                {
                    if(phase == "All")
                    {
                        deliverySequences = db.DeliverySequences.Where(x => x.Status != "...").GroupBy(x => x.OrderId)
                            .SelectMany(g => g.OrderByDescending(x => x.Sequence).Take(1)).ToList();
                    }
                    else
                    {
                        Port port = db.Ports.FirstOrDefault(x => x.Name == phase);
                        User u = db.Users.FirstOrDefault(x => x.PortId == port.Id);
                        deliverySequences = db.DeliverySequences.Where(x => x.UserId == u.Id && x.Status != "...").GroupBy(x => x.OrderId)
                          .SelectMany(g => g.OrderByDescending(x => x.Sequence).Take(1)).ToList();
                    }
                }
                else
                {
                    if (phase == "All")
                    {
                        deliverySequences = db.DeliverySequences.Where(x => x.Status != "...").GroupBy(x => x.OrderId)
                          .SelectMany(g => g.OrderByDescending(x => x.Sequence).Take(1).Where(x => x.Status == category)).ToList();
                    }
                    else
                    {
                        Port port = db.Ports.FirstOrDefault(x => x.Name == phase);
                        User u = db.Users.FirstOrDefault(x => x.PortId == port.Id);
                        deliverySequences = db.DeliverySequences.Where(x => x.Status == category && x.UserId == u.Id).GroupBy(x => x.OrderId)
                         .SelectMany(g => g.OrderByDescending(x => x.Sequence).Take(1)).ToList();
                    }
                
                }
            }

            else if (user.UserType == "Staff")
            {
                if (category == "All")
                    deliverySequences = db.DeliverySequences.Where(x => x.Status != "..." && x.UserId == user.Id).ToList();
                else
                    deliverySequences = db.DeliverySequences.Where(x => x.Status == category && x.UserId == user.Id).ToList();
            }

            List<Order> orders = new List<Order>();
            foreach(DeliverySequence deliverySequence in deliverySequences)
            {
                Order order = db.Orders.FirstOrDefault(x => x.Id == deliverySequence.OrderId);
                orders.Add(order);
            }

            return orders;
        }

        [Route("GetAll")]
        [HttpGet]
        public IEnumerable<Order> GetAll()
        {
            return db.Orders.ToList();
        }

        [Route("Get")]
        [HttpGet]
        public Order Get(Guid id)
        {
            return db.Orders.FirstOrDefault(x => x.Id == id);
        }

        [Route("GetCurrentMax")]
        [HttpGet]
        public Order GetCurrentMax()
        {
            int max = db.Orders.Max(x => x.Counter);
            return db.Orders.FirstOrDefault(x => x.Counter == max);
        }

        [Route("Add")]
        [HttpPost]
        public void Add([FromBody] OrderSet value)
        {
            int max = 0;

            try
            {
               max = db.Orders.Max(x => x.Counter);
            }
            catch(Exception e)
            {
                max = 1;
            }

            value.Order.Counter = max + 1;
            value.Order.SoNo = "SO - " + (1000 + value.Order.Counter);
            value.Order.Id = Guid.NewGuid();
            value.Order.DateOrdered = DateTime.Now;
            value.Order.DateTransfered = DateTime.Now;
            db.Orders.Add(value.Order);

            foreach(OrderDetail od in value.OrderDetails)
            {
                od.Id = Guid.NewGuid();
                od.OrderId = value.Order.Id;
                db.OrderDetails.Add(od);
            }


            DeliverySequence ds1 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("2c2ed895-e8a2-4234-d586-050f903d2d64"),
                Sequence = 1,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "Pending",
            };
            DeliverySequence ds2 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("2c2ed895-e8a2-4234-d586-050f903d2d65"),
                Sequence = 2,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            DeliverySequence ds3 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("2c2ed895-e8a2-4234-d586-050f903d2d66"),
                Sequence = 3,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            DeliverySequence ds4 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("2c2ed895-e8a2-4234-d586-050f903d2d67"),
                Sequence = 4,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            DeliverySequence ds5 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("2c2ed895-e8a2-4234-d586-050f903d2d68"),
                Sequence = 5,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            DeliverySequence ds6 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("2c2ed895-e8a2-4234-d586-050f903d2d69"),
                Sequence = 6,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            DeliverySequence ds7 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("1c2ed895-e8a2-4234-d586-050f903d2d63"),
                Sequence = 7,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            DeliverySequence ds8 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("1c2ed895-e8a2-4234-d586-050f903d2d64"),
                Sequence = 8,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            DeliverySequence ds9 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("1c2ed895-e8a2-4234-d586-050f903d2d65"),
                Sequence = 9,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            DeliverySequence ds10 = new DeliverySequence()
            {
                Id = Guid.NewGuid(),
                OrderId = value.Order.Id,
                UserId = Guid.Parse("1c2ed895-e8a2-4234-d586-050f903d2d66"),
                Sequence = 10,
                DateReceived = DateTime.Now,
                DateTransfered = DateTime.Now,
                Status = "...",
            };
            db.DeliverySequences.Add(ds1);
            db.DeliverySequences.Add(ds2);
            db.DeliverySequences.Add(ds3);
            db.DeliverySequences.Add(ds4);
            db.DeliverySequences.Add(ds5);
            db.DeliverySequences.Add(ds6);
            db.DeliverySequences.Add(ds7);
            db.DeliverySequences.Add(ds8);
            db.DeliverySequences.Add(ds9);
            db.DeliverySequences.Add(ds10);

            db.SaveChanges();
        }

        [Route("AddMany")]
        [HttpPost]
        public void AddMany([FromBody] IEnumerable<Order> value)
        {
            foreach (var v in value)
            {
                db.Orders.Add(v);
            }
            db.SaveChanges();
        }


        [Route("Update")]
        [HttpPut]
        public void Update([FromBody] Order value)
        {
            var v = db.Orders.FirstOrDefault(x => x.Id == value.Id);
            db.Orders.Remove(v);
            db.Orders.Add(value);
            db.SaveChanges();
        }

        [Route("Delete")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            db.Orders.Remove(db.Orders.FirstOrDefault(x => x.Id == id));
            db.SaveChanges();
        }
    }
}