using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UserLaundry.Service
{
    public static class Service
    {
        private static readonly LaundryDBEntities1 Db = Dao.Dao.GetDbEntities();

        public static LaundryUser FindLaundryUser(String name)
        {
            return Dao.Dao.FindLaundryUser(name);
        }

        public static List<Machine> FindMachinesAvailable(LaundryRoom laundryRoom, Reservation reservation)
        {
            return Dao.Dao.FindMachinesAvailable(laundryRoom, reservation);
        }

        public static WashTime FindWashTime(int id)
        {
            return Dao.Dao.FindWashTime(id);
        }

        public static Reservation CreateReservation(LaundryUser laundryUser, DateTime date)
        {
            Reservation tempReservation = new Reservation();
            tempReservation.LaundryUser1 = laundryUser;
            tempReservation.reservationDate = date;
            tempReservation.reservationUsed = false;
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
            WrapperMachineRe wrapper = new WrapperMachineRe();
            wrapper.Machine = machine.id;
            wrapper.Reservation = reservation.id;
            //reservation.Machine = machine.id;
            Db.WrapperMachineRes.Add(wrapper);
            Db.SaveChanges();
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

        public static void StartWash(Reservation r)
        {
            r.reservationUsed = true;
            Db.SaveChanges();
        }

        public static StartedWashCost CreateStartedWashCost(Reservation reservation, MachineProgram machineProgram)
        {
            StartedWashCost cost = new StartedWashCost();
            cost.Reservation1 = reservation;
            cost.MachineProgram1 = machineProgram;
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
    }
}