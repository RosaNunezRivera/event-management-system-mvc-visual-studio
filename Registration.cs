using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMSENTITIES
{
    public class RegistrationColletion
    {
        public int RegistrationID { get; set; }
        public int EventID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
