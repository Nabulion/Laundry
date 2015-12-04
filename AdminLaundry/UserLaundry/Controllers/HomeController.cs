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

                return RedirectToAction("PickWashTime", new { userid = userid, resid = r.id });
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


        public ActionResult Reservation(String userid, int washid, int resid)
        {
            Wrapper w = new Wrapper();
            Reservation r = Service.Service.FindReservation(resid);
            WashTime washTime = Service.Service.FindWashTime(washid);
            Service.Service.AddWashTimeReservation(r, washTime);

            w.WashTime = washTime;
            w.Reservation = r;
            w.LaundryUser = Service.Service.FindLaundryUser(userid);
            
            w.Machines = Service.Service.FindMachinesAvailable(w.LaundryUser.LaundryRoom1, r);
            return View(w);
        }

        public ActionResult Reserved(int resid, int machineid)
        {
            Reservation r = Service.Service.FindReservation(resid);
            Machine m = Service.Service.FindMachine(machineid);
            Service.Service.AddMachineReservation(r, m);
            return RedirectToAction("Reservation", new{userid = r.LaundryUser1.name, washid = r.WashTime, resid = r.id});
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

        public ActionResult Start(int id, int programid)
        {
            Reservation r = Service.Service.FindReservation(id);
            MachineProgram program = Service.Service.FindProgram(programid);
            Service.Service.StartWash(r, program.Machine1);
            Service.Service.CreateStartedWashCost(r, program);
            return RedirectToAction("StartWash", new { id = r.id });
        }

        public ActionResult LaundryUserOverview(String userid)
        {
            LaundryUser laundryUser = Service.Service.FindLaundryUser(userid);
            return View(laundryUser);
        }

        public ActionResult Back(int resid)
        {
            Reservation r = Service.Service.FindReservation(resid);
            Service.Service.DeleteResWithNulls(r);

            return RedirectToAction("PickDate", new {userid = r.LaundryUser1.name});
        }
    }
}