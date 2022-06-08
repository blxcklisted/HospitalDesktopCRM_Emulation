namespace Project.DAL.Context
{
    public class Drug
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int? ManufacturerId { get; set; }
        public int? CategoryId { get; set; }
        public byte[] Image { get; set; }
        public int? DrugCount { get; set; }
        public decimal? DrugPrice { get; set; }

        public virtual Category Category { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
