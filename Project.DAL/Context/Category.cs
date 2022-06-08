using System.Collections.Generic;

namespace Project.DAL.Context
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<Drug> Drugs { get; set; } = new HashSet<Drug>();
    }
}
