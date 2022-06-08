namespace Project.BLL.DTO
{
    public class DrugDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ManufacturerName { get; set; }
        public string CategoryName { get; set; }
        public int? DrugCount { get; set; }
        public decimal? DrugPrice { get; set; }
        public byte[] Image { get; set; }
    }
}
