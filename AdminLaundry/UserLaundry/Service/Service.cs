using System;
using System.Data.Entity.Infrastructure;


namespace UserLaundry.Service
{
    public static class Service
    {
        private static readonly LaundryDBEntities2 Db = Dao.Dao.GetDbEntities();

        public static LaundryUser FindLaundryUser(String name)
        {
            return Dao.Dao.FindLaundryUser(name);
        }

        public static WashTime FindWashTime(int id)
        {
            return Dao.Dao.FindWashTime(id);
        }

        public static Reservation CreateReservation(LaundryUser laundryUser, DateTime date)
        {
            Reservation tempReservation = new Reservation
            {
                LaundryUser1 = laundryUser,
                reservationDate = date,
                reservationUsed = false,
                inactive = false
            };
            Db.Reservations.Add(tempReservation);
            Db.SaveChanges();
            return tempReservation;
        }

        public static void AddWashTimeReservation(Reservation reservation, WashTime washTime)
        {
                reservation.WashTime1 = washTime;
                Db.SaveChanges();
        }




        public static void AddMachineReservation(Reservation reservation, Machine machine)
        {
                
            try
            {
                reservation.Machines.Add(machine);
                Db.SaveChanges();

            }
            catch (DbUpdateException e)
            {
                
                Db.Entry(reservation).Reload();
            }
                
        }

        public static DateTime ValidateDate(String date)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(date);
            }
            catch
            {

                throw new Exception("Incorrect date format should be like day/month/year");
            }
            if (dateTime >= DateTime.Today.AddDays(7) || dateTime < DateTime.Today)
                throw new Exception("Date not within correct range (7 days)");
            return dateTime;
        }

        public static Reservation FindReservation(int id)
        {
            return Dao.Dao.FindReservation(id);
        }

        public static Machine FindMachine(int machineid)
        {
            return Dao.Dao.FindMachine(machineid);
        }

        public static void StartWash(Reservation r, Machine m)
        {
            r.reservationUsed = true;
            m.start = true;
            r.reservationDate = DateTime.Now;
            Db.SaveChanges();
        }

        public static StartedWashCost CreateStartedWashCost(Reservation reservation, MachineProgram machineProgram)
        {
            StartedWashCost cost = new StartedWashCost();
            cost.MachineProgram1 = machineProgram;
            
            reservation.StartedWashCosts.Add(cost);
            Db.StartedWashCosts.Add(cost);
            Db.SaveChanges();
            return cost;
        }

        public static void DeleteReservation(Reservation r)
        {
            if (r != null)
            {
                Db.Reservations.Remove(r);
                Db.SaveChanges();
            }
        }

        public static MachineProgram FindProgram(int programid)
        {
            return Dao.Dao.FindProgram(programid);
        }

        public static void DeleteResWithNulls(Reservation reservation)
        {
            if (reservation != null)
            {
                if (reservation.WashTime1 == null || reservation.Machines.Count == 0)
                {
                    Db.Reservations.Remove(reservation);
                    Db.SaveChanges();
                }
            }
        }

        public static void SetInactiveResFromMachinePastDate(int min)
        {
            Dao.Dao.SetInactiveResFromMachinePastDate(min);
        }

        public static void MachineFinished(Machine m)
        {
            m.start = false;
            Db.SaveChanges();
        }
    }
}