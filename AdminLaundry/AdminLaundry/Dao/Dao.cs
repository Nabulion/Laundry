using System.Collections.Generic;
using System.Linq;


namespace AdminLaundry.Dao
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
