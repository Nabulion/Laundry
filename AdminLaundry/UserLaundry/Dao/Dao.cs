using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UserLaundry.Dao
{
    public static class Dao
    {
        private static LaundryDBEntities2 _db = null;

        public static LaundryDBEntities2 GetDbEntities()
        {
            if (_db == null)
            {
                _db = new LaundryDBEntities2();
            }
            return _db;
        }

        public static LaundryUser FindLaundryUser(String name)
        {
            return GetDbEntities().LaundryUsers.Find(name);
        }

        public static List<Machine> FindMachinesAvailable(LaundryRoom laundryRoom, Reservation reservation)
        {
            List<Machine> machines = new List<Machine>();
            foreach (var m in laundryRoom.Machines)
            {
                if (m.Reservations.Count == 0)
                {
                    if (!machines.Contains(m))
                    machines.Add(m);
                }
                else
                {
                    foreach (var res in m.Reservations)
                    {
                        if (res.reservationDate != reservation.reservationDate &&
                            res.WashTime1 == reservation.WashTime1)
                        {
                            if(!machines.Contains(m))
                            machines.Add(m);
                        }
                        else if (res.reservationDate == reservation.reservationDate &&
                          res.WashTime1 != reservation.WashTime1)
                        {
                            if (!machines.Contains(m))
                            machines.Add(m);
                        }
                        else if (res.reservationDate != reservation.reservationDate &&
                                 res.WashTime1 != reservation.WashTime1)
                        {
                            if (!machines.Contains(m))
                            machines.Add(m);
                        }

                    }
                }
            }

            return machines;
        }

        public static WashTime FindWashTime(int id)
        {
            return _db.WashTimes.Find(id);
        }

        public static Reservation FindReservation(int id)
        {
            return _db.Reservations.Find(id);
        }

        public static Machine FindMachine(int machineid)
        {
            return _db.Machines.Find(machineid);
        }

        public static MachineProgram FindProgram(int programid)
        {
            return _db.MachinePrograms.Find(programid);
        }
    }
}
