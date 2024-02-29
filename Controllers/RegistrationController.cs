using EMSBLL;
using EMSENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Collections;
using event_management_system_mvc.Models;


namespace event_management_system_mvc.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
     
        public ActionResult Index(int eventId)
        {
            EventService es = new EventService();
            var eventById = es.GetEventById(eventId);

            TempData["eventId"] = eventId;
            RegistrationService rs = new RegistrationService();
            List<EMSENTITIES.Registration> registrationsByEventID = new List<EMSENTITIES.Registration>();
            
            var registrations = rs.GetRegistrations(eventId);
            registrationsByEventID.AddRange(registrations);

            ViewBag.Message = TempData["Message"];
            var viewModel1 = new CompositeViewModel
            {
                EventsCol = eventById,
                RegistrationsCol = registrationsByEventID
            };

            return View(viewModel1);
        }


        public ActionResult CreateRegistration()
        {
            Registration reg = null;
          
            // Get eventId from TempData
            int eventId = (int)TempData["eventId"];  

            if (eventId != 0)
            {
                reg = new Registration()  
                {
                    EventID = eventId,
                    RegistrationDate = null
                };
            }
            return View(reg);
        }

        [HttpPost]
        public ActionResult CreateRegistration(Registration reg)
        {
            RegistrationService rs = new RegistrationService();

            if (rs.AddRegistrationService(reg))
            {
                return RedirectToAction("Index", new { eventId = reg.EventID });
            }
            else
            {
                return View(reg);
            }
        }

        public ActionResult EditRegistrationView(int registrationId)
        {
            RegistrationService rs = new RegistrationService();
            int eventId = (int)TempData["eventId"];
            var reg = rs.GetRegistrations(eventId).Find(x => x.RegistrationID == registrationId);
            return View(reg);
        }

        [HttpPost]
        public ActionResult EditRegistrationView(Registration reg)
        {
            RegistrationService rs = new RegistrationService();
            if (rs.UpdateRegistrationService(reg))
            {
                return RedirectToAction("Index", new { eventId = reg.EventID });
            }
            else
            {
                return View(reg);
            }
        }

        public ActionResult DeleteRegistration(int registrationId)
        {
            RegistrationService rs = new RegistrationService();
            int eventId = (int)TempData["eventId"];
            var regObj = rs.GetRegistrations(eventId).Find(x => x.RegistrationID == registrationId);
           
            if (rs.DeleteRegistrationService(registrationId, regObj.EventID))
            {
                return RedirectToAction("Index", new { eventId = eventId });
            }
            return null;
        }
    }
}