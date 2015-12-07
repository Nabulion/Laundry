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

        public static void RemoveResFromMachinePastDate(int min)
        {
            foreach (var machine in _db.Machines)
            {
                for (int i = 0; i < machine.Reservations.Count; i++)
                {
                    if (!machine.Reservations.ToList()[i].reservationUsed.GetValueOrDefault())
                    {
                        DateTime expireDate = machine.Reservations.ToList()[i].reservationDate.GetValueOrDefault() 
                                            + machine.Reservations.ToList()[i].WashTime1.fromTime.GetValueOrDefault();
                        if (DateTime.Now > expireDate.AddMinutes(min))
                        {
                            machine.Reservations.ToList()[i].inactive = true;
                        }
                    }
                }
            }
            _db.SaveChanges();
        }

    }
}
