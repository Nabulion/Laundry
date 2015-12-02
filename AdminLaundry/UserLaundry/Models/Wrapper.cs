using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UserLaundry;

namespace UserLaundry.Models
{
    public class Wrapper
    {
        public LaundryUser LaundryUser { get; set; }
        public WashTime WashTime { get; set; }
        public List<Machine> Machines { get; set; }
    }
}