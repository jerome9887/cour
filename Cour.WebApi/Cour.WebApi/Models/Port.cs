using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cour.WebApi.Models
{
    public class Port
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}