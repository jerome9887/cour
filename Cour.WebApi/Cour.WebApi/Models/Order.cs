using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cour.WebApi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string SoNo { get; set; }
        public DateTime? DateOrdered { get; set; }
        public DateTime? DateTransfered { get; set; }
        public string Status { get; set; }
        public Guid CustomerId { get; set; }
        public int Counter { get; set; }
        public string Address { get; set; }
    }
}