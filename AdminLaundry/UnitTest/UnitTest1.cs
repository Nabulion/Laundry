using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserLaundry;
using UserLaundry.Dao;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private UserLaundry.LaundryRoom laundryRoom;
        private UserLaundry.Reservation reservation;
        private UserLaundry.LaundryUser laundryUser;
        private UserLaundry.WashTime washTime;

        [TestInitialize]
        public void Init()
        {
            laundryRoom = new LaundryRoom();
            laundryRoom.Machines.Add(new Machine());
            laundryRoom.Machines.Add(new Machine());
            laundryRoom.Machines.Add(new Machine());
            laundryUser = new LaundryUser();
            laundryUser.name = "Namjoo";

            reservation = new Reservation();
           
            washTime = new WashTime();
            washTime.fromTime = DateTime.Now.AddMinutes(10).TimeOfDay;
       
            reservation.WashTime1 = washTime;
            laundryUser.Reservations.Add(reservation);
        }

        //LaundryRoom
        [TestMethod]
        public void TestFindMachinesAvailable1()
        {
            reservation.reservationDate = DateTime.Today;
            Assert.AreEqual(3, laundryRoom.FindMachinesAvailable(reservation).Count);
        }

        [TestMethod]
        public void TestFindMachinesAvailable2()
        {
            reservation.reservationDate = DateTime.Today;
            reservation.WashTime1 = washTime;

            laundryRoom.Machines.LastOrDefault().Reservations.Add(reservation);

            Assert.AreEqual(2, laundryRoom.FindMachinesAvailable(reservation).Count);
        }

        //LaundryUser
        [TestMethod]
        public void TestPaidWashes()
        {
            reservation.reservationUsed = true;
            StartedWashCost start1 = new StartedWashCost();
            MachineProgram program = new MachineProgram();
            
            start1.MachineProgram1 = program;
            program.price = 10;
            
            reservation.StartedWashCosts.Add(start1);
            start1.payed = true;
           
            StartedWashCost start2 = new StartedWashCost();
            start2.MachineProgram1 = program;
      
            reservation.StartedWashCosts.Add(start2);
            start2.payed = true;

            Assert.AreEqual(20, laundryUser.PaidWashes());
        }

        [TestMethod]
        public void TestUnPaidWashes1()
        {
            reservation.reservationUsed = true;
            StartedWashCost start1 = new StartedWashCost();
            MachineProgram program = new MachineProgram();

            start1.MachineProgram1 = program;
            program.price = 10;

            reservation.StartedWashCosts.Add(start1);
            

            StartedWashCost start2 = new StartedWashCost();
            start2.MachineProgram1 = program;

            reservation.StartedWashCosts.Add(start2);
            

            Assert.AreEqual(20, laundryUser.UnPaidWashes());

        }
        
        [TestMethod]
        public void TestUnPaidWashes2()
        {
            reservation.reservationUsed = true;
            StartedWashCost start1 = new StartedWashCost();
            MachineProgram program = new MachineProgram();

            start1.MachineProgram1 = program;
            program.price = 10;

            reservation.StartedWashCosts.Add(start1);


            StartedWashCost start2 = new StartedWashCost();
            start2.MachineProgram1 = program;
            start2.payed = true;
           
            reservation.StartedWashCosts.Add(start2);


            Assert.AreEqual(10, laundryUser.UnPaidWashes());
        }

        [TestMethod]
        public void TestGetTodaysRes1()
        {
            reservation.reservationDate = DateTime.Today;
            laundryUser.Reservations.Add(reservation);
            Assert.AreEqual(1, laundryUser.GetTodaysRes().Count);
        }

        [TestMethod]
        public void TestGetTodaysRes2()
        {
            reservation.reservationDate = DateTime.Today;
            Reservation futureReservation = new Reservation();
            futureReservation.reservationDate = DateTime.Today.AddDays(7);
            futureReservation.WashTime1 = washTime;
            laundryUser.Reservations.Add(reservation);
            laundryUser.Reservations.Add(futureReservation);

            Assert.AreEqual(1, laundryUser.GetTodaysRes().Count);
        }

        [TestMethod]
        public void TestGetFutureRes()
        {
            reservation.reservationDate = DateTime.Today;
            Reservation futureReservation = new Reservation();
            futureReservation.reservationDate = DateTime.Today.AddDays(7);
            laundryUser.Reservations.Add(futureReservation);

            Assert.AreEqual(1, laundryUser.GetFutureRes().Count);
        }

        //Reservation
        [TestMethod]
        public void TestFindStartedWash()
        {
            reservation.reservationUsed = true;
            StartedWashCost start1 = new StartedWashCost();
            start1.id = 1;
            MachineProgram program1 = new MachineProgram();
            MachineProgram program2 = new MachineProgram();
            start1.MachineProgram1 = program1;
            program1.price = 10;
            program2.price = 12;
            reservation.StartedWashCosts.Add(start1);


            StartedWashCost start2 = new StartedWashCost();
            start2.MachineProgram1 = program2;
            start2.payed = true;
            start2.id = 2;
            reservation.StartedWashCosts.Add(start2);

            Assert.AreEqual(start1.id, reservation.findStartedWash(program1).id);     
        }
    }
}
