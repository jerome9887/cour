using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cour.WebApi.Models
{
    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}