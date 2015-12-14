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

       public static void UserTotalCostPayed(LaundryUser user)
       {
           foreach (StartedWashCost start in user.Reservations.Where(res => res.reservationUsed.GetValueOrDefault()).SelectMany(res => res.StartedWashCosts))
           {
               start.payed = true;
           }
           Db.SaveChanges();
       }

       public static List<LaundryUser> GetUsers(LaundryRoom laundryRoom)
       {
           return laundryRoom.LaundryUsers.ToList();
       }
   }
}
