using System;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Web.Mvc;


namespace UserLaundry.Controllers
{
    public class HomeController : Controller
    {
        private const int MinPastResTime = 15;
        // GET: Home
        public ActionResult Index()
        {
            Service.Service.SetInactiveResFromMachinePastDate(MinPastResTime);

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            String name = fc["name"];
            try
            {
                LaundryUser laundryUser = Service.Service.FindLaundryUser(name);

                return RedirectToAction("PickDate", new { userid = laundryUser.name });

            }
            catch (Exception)
            {

                ModelState.AddModelError("LoginError", "no user found with that name");
                return View("");
            }

        }

        public ActionResult PickDate(String userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);

            return View(laundryUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PickDate(String userid, FormCollection fc)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);

            try
            {
                DateTime date = Service.Service.ValidateDate(fc["date"]);

                Reservation r = Service.Service.CreateReservation(laundryUser, date);

                return RedirectToAction("PickWashTime", new { userid = laundryUser.name, resid = r.id });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("DateError", e.Message);

                return View(laundryUser);
            }
        }

        public ActionResult PickWashTime(String userid, int resid)
        {
                LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);

                return View(laundryUser);
        }

        public ActionResult WashTimePicked(int washid, int resid)
        {
            Reservation r = Service.Service.FindReservation(resid);
            WashTime washTime = Service.Service.FindWashTime(washid);

            Service.Service.AddWashTimeReservation(r, washTime);

            return RedirectToAction("Reservation", new { userid = r.LaundryUser });
        }


        public ActionResult Reservation(String userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);

            return View(laundryUser);
        }

        public ActionResult Reserved(int resid, int machineid)
        {
            Reservation r = Service.Service.FindReservation(resid); ;
            try
            {
                TransactionOptions options =
             new TransactionOptions
             {
                 IsolationLevel =
                 IsolationLevel.Serializable,
                 Timeout = TransactionManager.DefaultTimeout
             };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
                {

                    Machine m = Service.Service.FindMachine(machineid);

                    Dao.Dao.GetDbEntities().Entry(m).Reload();

                    if ((from machine in m.LaundryRoom1.Machines from res in machine.Reservations where res.reservationDate.GetValueOrDefault().Date == r.reservationDate.GetValueOrDefault().Date && res.WashTime == r.WashTime select res).Any())
                    {
                        //do nothing or you could throw an error but then it wouldnt be the transaction handling the change...
                    }
                   // Thread.Sleep(5000);

                    Service.Service.AddMachineReservation(r, m);
                    scope.Complete();

                }
            }
            catch (TransactionAbortedException e)
            {
                Service.Service.DeleteResWithNulls(r);
                ModelState.AddModelError("DateError", "The machine you picked has been taken by someone else please try another date and machine");
                return View("PickDate", r.LaundryUser1);
            }

            return RedirectToAction("Reservation",
                    new { userid = r.LaundryUser1.name});
        }

        public ActionResult AllReservations(String userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);
            Service.Service.SetInactiveResFromMachinePastDate(MinPastResTime);

            return View(laundryUser);
        }

        public ActionResult StartWash(int resid)
        {
            Reservation r = Service.Service.FindReservation(resid);

            return View(r);
        }

        public ActionResult Start(int resid, int programid)
        {

            Reservation r = Service.Service.FindReservation(resid);
            MachineProgram program = Service.Service.FindProgram(programid);
            Service.Service.StartWash(r, program.Machine1);
            Service.Service.CreateStartedWashCost(r, program);

            return RedirectToAction("StartWash", new { resid = r.id });
        }

        public ActionResult LaundryUserOverview(String userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);

            return View(laundryUser);
        }

        public ActionResult Back(int resid, String userid)
        {
            Reservation r = Service.Service.FindReservation(resid);
            Service.Service.DeleteResWithNulls(r);

            return RedirectToAction("PickDate", new { userid });
        }

        public ActionResult TakeClothOut(int machine, int resid)
        {
            Machine m = Service.Service.FindMachine(machine);
            Service.Service.MachineFinished(m);

            return RedirectToAction("StartWash", new { resid });
        }

        public ActionResult SeeTodaysAvailableMacinhes(string userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);
            try
            {
                Reservation r = Service.Service.CreateReservation(laundryUser, DateTime.Today);
                Service.Service.AddWashTimeReservation(r, laundryUser.LaundryRoom1.FindWashTime());
            }
            catch (Exception e)
            {
                return View("PickDate", laundryUser);
            }

            return RedirectToAction("TodaysMachinesReady", new { userid = laundryUser.name });
        }

        public ActionResult TodaysMachinesReady(String userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);

            return View(laundryUser);
        }

        public ActionResult TodaysMachines(int resid, int machineid)
        {
            Reservation r = Service.Service.FindReservation(resid); ;
            try
            {
                TransactionOptions options =
             new TransactionOptions
             {
                 IsolationLevel =
                 IsolationLevel.Serializable,
                 Timeout = TransactionManager.DefaultTimeout
             };

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
                {

                    Machine m = Service.Service.FindMachine(machineid);

                    Dao.Dao.GetDbEntities().Entry(m).Reload();

                    if ((from machine in m.LaundryRoom1.Machines from res in machine.Reservations where res.reservationDate.GetValueOrDefault().Date == r.reservationDate.GetValueOrDefault().Date && res.WashTime == r.WashTime select res).Any())
                    {
                        //do nothing or you could throw an error but then it wouldnt be the transaction handling the change...
                    }
                    //Thread.Sleep(5000);

                    Service.Service.AddMachineReservation(r, m);
                    scope.Complete();

                }
            }
            catch (TransactionAbortedException e)
            {
                Service.Service.DeleteResWithNulls(r);
                ModelState.AddModelError("DateError", "The machine you picked has been taken by someone else please try another date and machine");
                return View("PickDate", r.LaundryUser1);
            }

            return RedirectToAction("TodaysMachinesReady",
                    new { userid = r.LaundryUser1.name, resid = r.id });
        }
    }
}