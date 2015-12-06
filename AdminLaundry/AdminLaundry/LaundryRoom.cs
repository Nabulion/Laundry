//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdminLaundry
{
    using System;
    using System.Collections.Generic;
    
    public partial class LaundryRoom
    {
        public LaundryRoom()
        {
            this.LaundryUsers = new HashSet<LaundryUser>();
            this.Machines = new HashSet<Machine>();
            this.WashTimes = new HashSet<WashTime>();
        }
    
        public string name { get; set; }
        public Nullable<int> maxReservationPerUser { get; set; }
    
        public virtual ICollection<LaundryUser> LaundryUsers { get; set; }
        public virtual ICollection<Machine> Machines { get; set; }
        public virtual ICollection<WashTime> WashTimes { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
