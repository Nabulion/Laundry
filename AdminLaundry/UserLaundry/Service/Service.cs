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
            reservation.Machine = machine.id;
            Db.WrapperMachineRes.Add(wrapper);
            Db.SaveChanges();
        }

        public static DateTime ValidateDate(String date)
        {
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(date);
                if(dateTime >= DateTime.Now.AddDays(7) || dateTime <= DateTime.Now)
                    throw new Exception("Date not within correct range (7 days)");
            }
            catch (Exception)
            {
                
                throw new Exception("Incorrect date format");
            }
            return dateTime;
        }

        public static Reservation FindReservation(int id)
        {
            return Dao.Dao.FindReservation(id);
        }

        public static Machine FindMachine(int? machineid)
        {
            return Dao.Dao.FindMachine(machineid);
        }

        public static void StartWash(Reservation r)
        {
            r.reservationUsed = true;
            Db.SaveChanges();
        }
    }
}