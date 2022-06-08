using System;

namespace Project.BLL.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsAppointed { get; set; }
        public string UserName { get; set; }

    }
}
