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
        /// <summary>
        /// Method to get events and registration to creating a composite model
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Get all events 
            EventService es = new EventService();
            var allEvents = es.GetEvents();

            //Get all registration for each event using the eventId 
            RegistrationService rs = new RegistrationService();
            List<Registration> allRegistrations = new List<Registration>();


            //Getting registrations for each event
            foreach (var item in allEvents)
            {
                var registrationsForEvent = rs.GetRegistrations(item.EventID);
                allRegistrations.AddRange(registrationsForEvent);
            }

            //Store the message to send in the view
            ViewBag.Message = TempData["Message"];

            //Creating a composite model of events and registration to show in the view
            var viewModel1 = new CompositeViewModel
            {
                EventsCol = allEvents,
                RegistrationsCol = allRegistrations
            };

            //Sending the composite model to the view
            return View(viewModel1);
        }

        public ActionResult CreateEventView()
        {
            return View();
        }

        /// <summary>
        /// Method to create a new event and send the feedbac to the user
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateEventView(Event e)
        {
            //Result of execution of SQL Create store procedure and bring feedback 
            EventService es = new EventService();
            if (es.AddEventService(e))
            {
                ViewBag.Message = "Event is added successfully";
            }
            else 
            {
                ViewBag.Message = "Event has not been added successfully";
            }
            //Return to the same view
            return View();
        }
        /// <summary>
        /// Method to send to the vire the event to edit
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public ActionResult EditEventView(int eventId)
        {
            //Get the object with the data's event found 
            EventService es = new EventService();
            var e = es.GetEvents().Find(x => x.EventID == eventId);
            //Send the event to the view 
            return View(e);
        }

        /// <summary>
        /// Method to edit events 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEventView(Event e)
        {
            //Result of execution of SQL update store procedure and bring feedback 
            EventService es = new EventService();
            if (es.UpdateEventService(e))
            {
                ViewBag.Message = "Event is updated successfully";
            }
            else
            {
                ViewBag.Message = "Event has not been updated successfully";
            }
            //Return to the same view
            return View();
        }

        /// <summary>
        /// Method to delete events 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public ActionResult DeleteEvent(int eventId)
        {
            //Get all registrations by eventId given as parameter 
            RegistrationService rs = new RegistrationService();
            var registrations = rs.GetRegistrations(eventId);

            //Result of exection of SQL delete store procedure and bring feedback 
            EventService es = new EventService();
            if (es.DeleteEventService(eventId))
            {
                //Redirect to Index method in Event controller
                return RedirectToAction("Index");
            }

            return null;
        }
    }
}