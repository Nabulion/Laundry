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
       private static LaundryDBEntities1 Db = Dao.Dao.GetDbEntities();

       public static LaundryUser CreateLaundryUser(LaundryRoom laundryRoom, String name)
       {
           LaundryUser tempLaundryUser = new LaundryUser();
           try
           {
               tempLaundryUser.LaundryRoom1 = laundryRoom;
               tempLaundryUser.name = name;
               Db.LaundryUsers.Add(tempLaundryUser);
               Db.SaveChanges();
           }
           catch (DbUpdateException e)
           {

           }
        
           return tempLaundryUser;
       }

       public static List<LaundryRoom> GetLaundryRooms()
       {
           return Dao.Dao.GetLaundryRooms();
       }
   }
}
