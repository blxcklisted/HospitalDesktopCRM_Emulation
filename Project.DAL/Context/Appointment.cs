using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DAL.Context
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsAppointed { get; set; }

        [Column("DoctorId")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
