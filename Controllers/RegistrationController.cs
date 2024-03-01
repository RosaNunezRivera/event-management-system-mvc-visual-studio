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
using WebGrease.Activities;


namespace event_management_system_mvc.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
     
        public ActionResult Index(int eventId)
        {
            //Get all events by EventId
            EventService es = new EventService();
            var eventById = es.GetEventById(eventId);

            //Store the iventId in tempData to get registration by eventId
            TempData["eventId"] = eventId;


            //Get all registrations by EventId
            RegistrationService rs = new RegistrationService();           
            var registrations = rs.GetRegistrations(eventId);
           
            //Adding the message in case to attemp events
            ViewBag.Message = TempData["Message"];

            //Creating a composite view 
            var viewModel1 = new CompositeViewModel
            {
                EventsCol = eventById,
                RegistrationsCol = registrations
            };

            //Sending the model to the view
            return View(viewModel1);
        }


        public ActionResult CreateRegistration()
        {
            Registration reg = null;
          
            // Get eventId from TempData
            int eventId = (int)TempData["eventId"];

            //Create a new registration object
            if (eventId != 0)
            {
                reg = new Registration()  
                {
                    EventID = eventId,
                    RegistrationDate = null
                };
            }
            //Sending the new registration object to the view 
            return View(reg);
        }

        /// <summary>
        /// Method to create registration called from the event view 
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateRegistration(Registration reg)
        {
            //Get all registrations
            RegistrationService rs = new RegistrationService();

            //Result of exection of SQL insert store procedure and show feedback
            if (rs.AddRegistrationService(reg))
            {
                ViewBag.Message = "Registration is added successfully";
            }
            else
            {
                ViewBag.Message = "Registration cannot be added successfully";
            }

            //Sending the new registration object to the view 
            return View(reg);
           
        }

        /// <summary>
        /// Create Registration with DropDown option
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateRegistrationAnyEvent()
        {
            //Get all events 
            EventService es = new EventService();
            var allEvents = es.GetEvents();

            //Create a DropDown using viewbag with eventId and EventName 
            ViewBag.AllEvents = new SelectList(allEvents, "EventId", "EventName");

            //Create a new registration object
            Registration reg = null;
            reg = new Registration()
            {
                RegistrationDate = null
            };

            //Sending the new registration object to the view 
            return View(reg);
        }

        /// <summary>
        /// Method to create a new registration
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateRegistrationAnyEvent(Registration reg)
        {
            RegistrationService rs = new RegistrationService();

            //Result of exection of SQL insert store procedure and show feedback
            if (rs.AddRegistrationService(reg))
            {
                ViewBag.Message = "Registration is added successfully";
            }
            else
            {
                ViewBag.Message = "Registration cannot be added successfully";
            }

            //Redirect to the method index in the controller Event to 
            //come back to the same view where this option was called 
            return RedirectToAction("Index", "Event");
           
        }

        /// <summary>
        /// Method to Edit a registration
        /// </summary>
        /// <param name="registrationId"></param>
        /// <returns></returns>
        public ActionResult EditRegistrationView(int registrationId)
        {
            // Get all registrations to show in the view
            RegistrationService rs = new RegistrationService();

            //Store eventId in tempdata to use to find the registration by eventId
            int eventId = (int)TempData["eventId"];

            //Registration collection with all the register with eventId given
            var reg = rs.GetRegistrations(eventId).Find(x => x.RegistrationID == registrationId);
            
            //Return to the view sending the registration object 
            return View(reg);
        }

        /// <summary>
        /// Method to edit a registration
        /// </summary>
        /// <param name="reg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRegistrationView(Registration reg)
        {
            //Get all registrations to show in the view
            RegistrationService rs = new RegistrationService();

            //Result of exection of SQL update store procedure and
            //redirect to the method index 
            if (rs.UpdateRegistrationService(reg))
            {
                return RedirectToAction("Index", new { eventId = reg.EventID });
            }
            else
            {
                return View(reg);
            }
        }

        /// <summary>
        /// Method to detele registrations by eventId and registrationId parameters
        /// </summary>
        /// <param name="registrationId"></param>
        /// <returns></returns>
        public ActionResult DeleteRegistration(int registrationId)
        {
            //Get all registrations to show in the view
            RegistrationService rs = new RegistrationService();

            //Store eventId in tempdata to use to find the registration by eventId
            int eventId = (int)TempData["eventId"];

            //Registration collection with all the register with eventId given
            var regObj = rs.GetRegistrations(eventId).Find(x => x.RegistrationID == registrationId);

            //Result of exection of SQL delete store procedure and
            //redirect to the method index 
            if (rs.DeleteRegistrationService(registrationId, regObj.EventID))
            {
                return RedirectToAction("Index", new { eventId = eventId });
            }

            //Return to the view 
            return null;
        }
    }
}