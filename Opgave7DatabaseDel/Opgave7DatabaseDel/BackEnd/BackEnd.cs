using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opgave7DatabaseDel.BackEnd
{
    public class BackEnd
    {
        LaundryDBEntities _db = new LaundryDBEntities();
        public LaundryUser LaundryUser { get; set; }
        public LaundryRoom LaundryRoom { get; set; }

        public int TotalResMachine(LaundryRoom laundryRoom)
        {
            return (from laundryUser in laundryRoom.LaundryUsers from reservation in laundryUser.Reservations from m in reservation.Machines select m).Count();
        }

        public void AllLundries()
        {
            foreach (LaundryRoom laundryRoom in _db.LaundryRooms)
            {
                Console.WriteLine(laundryRoom.name + " has " + TotalResMachine(laundryRoom) + " machines in reservations");
            }
            
        }

        public void RemoveFutureRes(LaundryUser laundryUser)
        {
           
            for (int i = 0; i < laundryUser.Reservations.Count; i++)
            {
                if (laundryUser.Reservations.ToList()[i].reservationDate > DateTime.Now)
                {
                    laundryUser.Reservations.Remove(laundryUser.Reservations.ToList()[i]);
                    _db.Reservations.Remove(laundryUser.Reservations.ToList()[i]);
                }
            }

            _db.SaveChanges();
        }

        public void TestData()
        {
            LaundryRoom = new LaundryRoom();
            LaundryRoom.name = "laundryroom 1";
            LaundryRoom.maxReservationPerUser = 456;

            Machine machine1 = new Machine();
            machine1.LaundryRoom1 = LaundryRoom;
            machine1.machineType = "TestVaskemaskine";
            machine1.timesUsed = 0;
            machine1.broken = false;
            Machine machine2 = new Machine();
            machine2.LaundryRoom1 = LaundryRoom;
            machine2.machineType = "TestVaskemaskine";
            machine2.timesUsed = 0;
            machine2.broken = false;
            Machine machine3 = new Machine();
            machine3.LaundryRoom1 = LaundryRoom;
            machine3.machineType = "TestVaskemaskine";
            machine3.timesUsed = 0;
            machine3.broken = false;


            LaundryUser = new LaundryUser();
            LaundryUser.name = "Eunji1";
            LaundryUser.LaundryRoom1 = LaundryRoom;



            Reservation r1 = new Reservation();
            r1.reservationDate = new DateTime(2015, 12, 05);
            r1.LaundryUser1 = LaundryUser;
            r1.Machines.Add(machine1);
            r1.Machines.Add(machine2);

            Reservation r2 = new Reservation();
            r2.reservationDate = new DateTime(2015, 12, 04);
            r2.LaundryUser1 = LaundryUser;
            r2.Machines.Add(machine1);

            Reservation r3 = new Reservation();
            r3.reservationDate = new DateTime(2015, 12, 03);
            r3.LaundryUser1 = LaundryUser;
            r3.Machines.Add(machine1);
            r3.Machines.Add(machine3);

            try
            {
                
                _db.LaundryRooms.Add(LaundryRoom);
                _db.LaundryUsers.Add(LaundryUser);
                _db.Machines.Add(machine1);
                _db.Machines.Add(machine2);
                _db.Machines.Add(machine3);
                _db.Reservations.Add(r1);
                _db.Reservations.Add(r2);
                _db.Reservations.Add(r3);
                _db.SaveChanges();
            }
            catch (DbUpdateException e)
            {

                Console.WriteLine(e.InnerException);
            }
        }

        public void DeleteTestData()
        {
            try
            {
                foreach (var res in _db.Reservations.Where(res => res.LaundryUser.Equals("Eunji1")))
                {
                    _db.Reservations.Remove(res);
                }
                foreach (var machine in _db.Machines.Where(machine => machine.machineType.Equals("TestVaskemaskine")))
                {
                    _db.Machines.Remove(machine);
                }
                _db.LaundryUsers.Remove(LaundryUser);
                _db.LaundryRooms.Remove(LaundryRoom);
                _db.SaveChanges();
                Console.WriteLine("TEST OBJECTS DELETED");

            }
            catch (DbUpdateException e)
            {

                Console.WriteLine(e.InnerException);
            }
           
        }
    }
}
