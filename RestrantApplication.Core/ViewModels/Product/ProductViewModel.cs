namespace RestrantApplication.Core.ViewModels.Product
{
    public record ProductViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Prise { get; set; }
        public decimal Stock { get; set; }
        public bool IsAvilable { get; set; }
        public virtual CategoryProductViewModel category { get; set; }
        public virtual PhotoProductViewModel Photo { get; set; }
    }
}
