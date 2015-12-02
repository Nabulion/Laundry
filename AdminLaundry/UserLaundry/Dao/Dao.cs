using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UserLaundry.Dao
{
   public static class Dao
   {
       private static LaundryDBEntities1 _db = null;

       public static LaundryDBEntities1 GetDbEntities()
       {
           if (_db == null)
           {
               _db = new LaundryDBEntities1();
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
           List<Machine> list = laundryRoom.Machines.ToList();
           foreach (var m in list)
           {
               if (m.WrapperMachineRes.Count == 0)
               {
                   machines.Add(m);
               }
               else
               {
                   foreach (var wrapper in m.WrapperMachineRes)
                   {
                       if (wrapper.Reservation1.WashTime != reservation.WashTime && wrapper.Reservation1.reservationDate != reservation.reservationDate)
                       {
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

       internal static Reservation FindReservation(int id)
       {
           return _db.Reservations.Find(id);
       }

       internal static Machine FindMachine(int? machineid)
       {
           return _db.Machines.Find(machineid);
       }
   }
}
