//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Linq;

namespace UserLaundry
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

        public List<Machine> GetMachinesInUse()
        {
            List<Machine> list = new List<Machine>();
            foreach (var machine in Machines)
            {
                if (machine.start.GetValueOrDefault())
                {
                    list.Add(machine);
                }
            }
            return list;
        }

        public List<Machine> FindMachinesAvailable(Reservation reservation)
        {
            List<Machine> machines = Machines.ToList();

            foreach (var m in Machines)
            {
                if (!m.broken.GetValueOrDefault())
                {
                    foreach (var res in m.Reservations)
                    {

                        if ((res.reservationDate == reservation.reservationDate &&
                             res.WashTime == reservation.WashTime && !res.inactive.GetValueOrDefault()) ||
                            checkIfMaxRes(reservation.LaundryUser1))
                        {
                            if (machines.Contains(m))
                            {
                                machines.Remove(m);
                            }
                        }


                    }
                }
                else
                {
                    if (machines.Contains(m))
                    {
                        machines.Remove(m);
                    }
                }
            }


            return machines;
        }

        public bool checkIfMaxRes(LaundryUser laundryUser)
        {
            int count = 0;
            foreach (var res in laundryUser.Reservations)
            {

                if (!res.reservationUsed.GetValueOrDefault())
                {
                    count += res.Machines.Count;
                }

            }
            return (count >= maxReservationPerUser.GetValueOrDefault());
        }

        public WashTime FindWashTime()
        {
            WashTime washTime = null;
            bool found = false;
            foreach (WashTime wt in WashTimes)
            {
                if (!found)
                {
                    if (DateTime.Today + wt.fromTime.GetValueOrDefault() >= DateTime.Now)
                    {
                        washTime = wt;
                        found = true;
                    }
                    else if (DateTime.Today + wt.toTime.GetValueOrDefault() >= DateTime.Now)
                    {
                        washTime = wt;
                        found = true;
                    }
                }
            }
            if (!found)
            {
                throw new Exception("Sorry its to late/early to start a machine");
            }
            return washTime;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
