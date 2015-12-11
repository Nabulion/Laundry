using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLaundry.Service
{
   public static class Service
   {
       private static LaundryDBEntities2 Db = Dao.Dao.GetDbEntities();

       public static LaundryUser CreateLaundryUser(LaundryRoom laundryRoom, String name)
       {
           LaundryUser tempLaundryUser = null;
           try
           {
               tempLaundryUser = new LaundryUser();
               tempLaundryUser.LaundryRoom1 = laundryRoom;
               tempLaundryUser.name = name;

               Db.LaundryUsers.Add(tempLaundryUser);
               Db.SaveChanges();
           }
           catch (DbUpdateException)
           {
               throw new Exception("There is already a user named: " + tempLaundryUser.name);
           }
           

           return tempLaundryUser;
       }

       public static List<LaundryRoom> GetLaundryRooms()
       {
           return Dao.Dao.GetLaundryRooms();
       }

       public static decimal UserTotalCost(LaundryUser user)
       {
           return (from res in user.Reservations 
                   where res.StartedWashCosts.Count != 0 from start in res.StartedWashCosts 
                   where start.payed.GetValueOrDefault() == false 
                   select start.MachineProgram1.price.GetValueOrDefault()).Sum();
       }

       public static void UserTotalCostPayed(LaundryUser user)
       {
           foreach (var start in user.Reservations.Where(res => res.StartedWashCosts.Count != 0).SelectMany(res => res.StartedWashCosts))
           {
               start.payed = true;
           }
           Db.SaveChanges();
       }

       public static List<LaundryUser> GetUsers()
       {
           return Dao.Dao.GetUsers();
       }
   }
}
