using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserLaundry.Models;

namespace UserLaundry.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            String name = fc["name"];
            LaundryUser laundryUser = Service.Service.FindLaundryUser(name);
            return RedirectToAction("PickDate", new { name = laundryUser.name });
        }

        public ActionResult PickDate(String name)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(name);
            return View(laundryUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PickDate(String name, FormCollection fc)
        {
            try
            {
                DateTime date = Service.Service.ValidateDate(fc["date"]);
                LaundryUser laundryUser = Service.Service.FindLaundryUser(name);

                Service.Service.CreateReservation(laundryUser, date);

                return RedirectToAction("UserPage", new { name = name });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("DateError", "The Date is not a real date try 10/10/2015");
                return RedirectToAction("PickDate", new { name = name });
            }
        }

        public ActionResult UserPage(String name)
        {
            try
            {
                LaundryUser laundryUser = Service.Service.FindLaundryUser(name);
                if (laundryUser == null) throw new Exception();
                return View(laundryUser);
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }

        }


        public ActionResult Reservation(String userid)
        {
            Wrapper w = new Wrapper();
            w.LaundryUser = Service.Service.FindLaundryUser(userid);
            Reservation r = w.LaundryUser.Reservations.LastOrDefault();

            WashTime washTime = r.WashTime1;
            w.WashTime = washTime;
            Service.Service.AddWashTimeReservation(r, washTime);
                
            w.Machines = Service.Service.FindMachinesAvailable(w.LaundryUser.LaundryRoom1, r);
            return View(w);
        }

        public ActionResult Reserved(int resid, int machineid)
        {
            Reservation r = Service.Service.FindReservation(resid);
            Machine m = Service.Service.FindMachine(machineid);
            Service.Service.AddMachineReservation(r, m);
            return RedirectToAction("Reservation", new{userid = r.LaundryUser1.name});
        }

        public ActionResult AllReservations(String userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);
            return View(laundryUser);
        }

        public ActionResult StartWash(int id)
        {
            Reservation r = Service.Service.FindReservation(id);
            return View(r);
        }

        public ActionResult Start(int id)
        {
            Reservation r = Service.Service.FindReservation(id);
            Service.Service.StartWash(r);
            return RedirectToAction("StartWash", new {id = r.id});
        }
    }
}