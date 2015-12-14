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

        public decimal PaidWashes()
        {
            decimal total = 0;
            foreach (var res in Reservations)
            {
                if (res.StartedWashCosts.Count != 0)
                {
                    foreach (var cost in res.StartedWashCosts)
                    {
                        if (cost.payed.GetValueOrDefault())
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
                        if (!cost.payed.GetValueOrDefault())
                        {
                            total += cost.MachineProgram1.price.GetValueOrDefault();
                        }
                    }
                }
            }
            return total;
        }

        public override string ToString()
        {
            return name;
        }

        public List<Reservation> GetFutureRes()
        {
            List<Reservation> list = new List<Reservation>();
            foreach (Reservation res in Reservations)
            {
                if (res != null)
                {
                    if (res.reservationDate.GetValueOrDefault().Date != DateTime.Today)
                    {
                        if (res.reservationDate.GetValueOrDefault().Date > DateTime.Today &&
                            !res.inactive.GetValueOrDefault())
                        {
                            list.Add(res);
                        }
                    }
                }
            }
            return list;
        }

        public List<Reservation> GetTodaysRes()
        {
            List<Reservation> list = new List<Reservation>();
            foreach (Reservation res in Reservations)
            {
                if (res != null)
                {
                    if (res.reservationDate.GetValueOrDefault().Date == DateTime.Today && !res.inactive.GetValueOrDefault())
                    {
                        DateTime resDate = res.reservationDate.GetValueOrDefault().Date +
                                           res.WashTime1.fromTime.GetValueOrDefault();
                        if (resDate >= DateTime.Now && !res.reservationUsed.GetValueOrDefault())
                        {
                            list.Add(res);
                        }
                    }
                }
            }
            return list;
        }

        public List<Reservation> GetTodaysUsedRes()
        {
            List<Reservation> list = new List<Reservation>();
            foreach (Reservation res in Reservations)
            {
                if (res != null)
                {
                    if (res.reservationDate.GetValueOrDefault().Date == DateTime.Today &&
                        res.reservationUsed.GetValueOrDefault())
                    {
                        list.Add(res);
                    }
                }
            }
            return list;
        }
    }
}
