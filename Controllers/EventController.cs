using EMSBLL;
using EMSENTITIES;
using event_management_system_mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace event_management_system_mvc.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        public ActionResult Index()
        {
            EventService es = new EventService();
            var allEvents = es.GetEvents();

            RegistrationService rs = new RegistrationService();
            List<EMSENTITIES.Registration> allRegistrations = new List<EMSENTITIES.Registration>();

            foreach (var item in allEvents)
            {
                var registrations = rs.GetRegistrations(item.EventID);
                allRegistrations.AddRange(registrations);
            }

            ViewBag.Message = TempData["Message"];
            var viewModel1 = new CompositeViewModel
            {
                EventsCol = allEvents,
                RegistrationsCol = allRegistrations
            };

            return View(viewModel1);
        }

        public ActionResult CreateEventView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEventView(Event e)
        {
            EventService es = new EventService();
            if (es.AddEventService(e))
            {
                ViewBag.Message = "Event is added successfully";
            }
            return View();
        }

        public ActionResult EditEventView(int eventId)
        {
                EventService es = new EventService();
                var e = es.GetEvents().Find(x => x.EventID == eventId);
                return View(e);
        }

        [HttpPost]
        public ActionResult EditEventView(Event e)
        {
            EventService es = new EventService();
            if (es.UpdateEventService(e))
            {
                ViewBag.Message = "Event is updated successfully";
            }
            return View();
        }

        public ActionResult DeleteEvent(int eventId)
        {
            RegistrationService rs = new RegistrationService();
            var registrations = rs.GetRegistrations(eventId);

            if (registrations.Any())
            {
                TempData["Message"] = "Event cannot be deleted because it has registrations.";
            }
            else 
            {
                EventService es = new EventService();
                if (es.DeleteEventService(eventId))
                {
                    TempData["Message"] = "Event deleted successfully.";
                }
                else
                {
                    TempData["Message"] = "Failed to delete event.";
                }
            }

            return RedirectToAction("Index");
        }
    }
}