using System;
using System.Linq;


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

 
        public static void SetInactiveResFromMachinePastDate(int min)
        {
            foreach (var res in _db.Reservations)
            {
                if (!res.reservationUsed.GetValueOrDefault())
                {
                    DateTime expireDate = res.reservationDate.GetValueOrDefault().Date
                                        + res.WashTime1.fromTime.GetValueOrDefault();
                    if (DateTime.Now > expireDate.AddMinutes(min))
                    {
                        res.inactive = true;
                    }
                }
            }
            _db.SaveChanges();
        }

    }
}
