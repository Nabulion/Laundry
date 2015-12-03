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
                   if(!machines.Contains(m))
                   machines.Add(m);
               }
               else
               {
                   foreach (var res in m.WrapperMachineRes)
                   {
                       if (res.Reservation1.reservationDate != reservation.reservationDate && 
                           res.Reservation1.WashTime1 == reservation.WashTime1)
                       {           
                           if(!machines.Contains(m))
                           machines.Add(m);
                       }
                       else if (res.Reservation1.reservationDate == reservation.reservationDate &&
                         res.Reservation1.WashTime1 != reservation.WashTime1)
                       {
                           if (!machines.Contains(m))
                           machines.Add(m);
                       }
                       else if (res.Reservation1.reservationDate != reservation.reservationDate &&
                                res.Reservation1.WashTime1 != reservation.WashTime1)
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
