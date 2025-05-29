namespace RestrantApplication.Core.ViewModels.Cart
{
    public record ProductcartViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Prise { get; set; }
        public decimal Stock { get; set; }
        public bool IsAvilable { get; set; }

        public virtual PhotoCartViewModel Photo { get; set; }
    }
}
