using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace event_management_system_mvc.Models
{
    public class CompositeViewModel
    {
        public IEnumerable<EMSENTITIES.Event> EventsCol { get; set; }
        public IEnumerable<EMSENTITIES.Registration> RegistrationsCol { get; set; }

        public int countRegistration(int eventId)
        {
            return RegistrationsCol.Count(r => r.EventID == eventId);
        }

        public string getEventName(int eventId)
        {
            var ev = EventsCol.FirstOrDefault(e => e.EventID == eventId);
            if (ev != null)
            {
                return ev.EventName;
            }
            else
            {
                return "Event Not Found";
            }
        }
    }

    
}