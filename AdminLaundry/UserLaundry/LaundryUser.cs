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
    
    public partial class LaundryUser
    {
        public LaundryUser()
        {
            this.Reservations = new HashSet<Reservation>();
        }
    
        public string name { get; set; }
        public string LaundryRoom { get; set; }
    
        public virtual LaundryRoom LaundryRoom1 { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

        public List<Reservation> GetNonUsedReservations()
        {
            List<Reservation> list = new List<Reservation>();
            foreach (var r in Reservations)
            {
                if (r.reservationUsed.GetValueOrDefault() == false)
                {
                    list.Add(r);
                }
            }
            return list;
        }

        public decimal PaidWashes()
        {
            decimal total = 0;
            foreach (var res in Reservations)
            {
                if (res.StartedWashCosts.Count != 0)
                {
                    foreach (var cost in res.StartedWashCosts)
                    {
                        if (cost.payed.GetValueOrDefault() == true)
                        {
                            total += cost.MachineProgram1.price.GetValueOrDefault();
                        }
                    }
                }
            }
            return total;
        }

        public decimal UnPaidWashes()
        {
            decimal total = 0;
            foreach (var res in Reservations)
            {
                if (res.StartedWashCosts.Count != 0)
                {
                    foreach (var cost in res.StartedWashCosts)
                    {
                        if (cost.payed.GetValueOrDefault() == false)
                        {
                            total += cost.MachineProgram1.price.GetValueOrDefault();
                        }
                    }
                }
            }
            return total;
        }
    }
}
