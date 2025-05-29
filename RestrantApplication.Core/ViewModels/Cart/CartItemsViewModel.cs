namespace RestrantApplication.Core.ViewModels.Cart
{
    public record CartItemsViewModel
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public decimal TotalAmount => Quantity * Product.Prise;
        public virtual ProductcartViewModel Product { get; set; }
    }
}
