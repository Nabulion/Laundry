//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UserLaundry
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public Reservation()
        {
            this.StartedWashCosts = new HashSet<StartedWashCost>();
            this.WrapperMachineRes = new HashSet<WrapperMachineRe>();
        }
    
        public int id { get; set; }
        public string LaundryUser { get; set; }
        public Nullable<System.DateTime> reservationDate { get; set; }
        public Nullable<int> WashTime { get; set; }
        public Nullable<int> Machine { get; set; }
        public Nullable<bool> reservationUsed { get; set; }
    
        public virtual LaundryUser LaundryUser1 { get; set; }
        public virtual WashTime WashTime1 { get; set; }
        public virtual ICollection<StartedWashCost> StartedWashCosts { get; set; }
        public virtual ICollection<WrapperMachineRe> WrapperMachineRes { get; set; }
        public virtual Machine Machine1 { get; set; }

        public override string ToString()
        {
            return reservationDate + "";
        }
    }
}