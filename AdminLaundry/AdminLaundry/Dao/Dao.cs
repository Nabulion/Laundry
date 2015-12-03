using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminLaundry.Dao
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

       public static List<LaundryRoom> GetLaundryRooms()
       {
           return GetDbEntities().LaundryRooms.ToList();
       }

       public static List<LaundryUser> GetUsers()
       {
           return _db.LaundryUsers.ToList();
       }
   }
}
