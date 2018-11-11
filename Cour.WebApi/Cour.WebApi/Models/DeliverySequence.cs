using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cour.WebApi.Models
{
    public class DeliverySequence
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public int Sequence { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateTransfered { get; set; }
        public string Status { get; set; }
    }
}