//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Opgave7DatabaseDel
{
    using System;
    using System.Collections.Generic;
    
    public partial class WashTime
    {
        public WashTime()
        {
            this.Reservations = new HashSet<Reservation>();
        }
    
        public int id { get; set; }
        public Nullable<System.TimeSpan> fromTime { get; set; }
        public Nullable<System.TimeSpan> toTime { get; set; }
        public string LaundryRoom { get; set; }
    
        public virtual LaundryRoom LaundryRoom1 { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
