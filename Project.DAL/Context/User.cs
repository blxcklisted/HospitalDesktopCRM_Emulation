using System.Collections.Generic;

namespace Project.DAL.Context
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
        public int? RoleId { get; set; }
        public int? AppointmentId { get; set; }//for User Role only

        public virtual Role Role { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();//for Doctor Role only
    }
}
