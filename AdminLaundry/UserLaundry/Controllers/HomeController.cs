using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using UserLaundry.Models;

namespace UserLaundry.Controllers
{
    public class HomeController : Controller
    {
        private const int MinPastResTime = 15;
        // GET: Home
        public ActionResult Index()
        {
            Service.Service.RemoveResFromMachinePastDate(MinPastResTime);
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
            try
            {
                Wrapper w = new Wrapper();

                LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);
                w.LaundryUser = laundryUser;
                Reservation r = Service.Service.FindReservation(resid);
                w.Reservation = r;
                return View(w);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }

        public ActionResult WashTimePicked(int washid, int resid)
        {
            Reservation r;
            TransactionOptions options =
             new TransactionOptions
             {
                 IsolationLevel =
                 IsolationLevel.Serializable,
                 Timeout = TransactionManager.DefaultTimeout
             };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            {
                WashTime washTime = Service.Service.FindWashTime(washid);
                r = Service.Service.FindReservation(resid);
                Service.Service.AddWashTimeReservation(r, washTime);
                scope.Complete();
                scope.Dispose();


            }
            return RedirectToAction("Reservation", new { userid = r.LaundryUser, resid = r.id });
        }


        public ActionResult Reservation(String userid, int resid)
        {
            Wrapper w = new Wrapper();
            w.Reservation = Service.Service.FindReservation(resid);
            w.LaundryUser = Service.Service.FindLaundryUser(userid);

            return View(w);
        }

        public ActionResult Reserved(int resid, int machineid)
        {
            Reservation r;

            TransactionOptions options =
             new TransactionOptions
             {
                 IsolationLevel =
                 IsolationLevel.Serializable,
                 Timeout = TransactionManager.DefaultTimeout
             };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, options))
            {
                r = Service.Service.FindReservation(resid);
                Machine m = Service.Service.FindMachine(machineid);
                Service.Service.AddMachineReservation(r, m);
                scope.Complete();
                scope.Dispose();
            }
            return RedirectToAction("Reservation",
                    new { userid = r.LaundryUser1.name, washid = r.WashTime, resid = r.id });
        }

        public ActionResult AllReservations(String userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);
            Service.Service.RemoveResFromMachinePastDate(MinPastResTime);
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
            return RedirectToAction("StartWash", new {resid});
        }
    }
}