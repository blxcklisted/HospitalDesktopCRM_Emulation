using System.Collections.Generic;

namespace Project.DAL.Context
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }

        public virtual ICollection<Drug> Drugs { get; set; } = new HashSet<Drug>();
    }
}
