namespace RestrantApplication.Core.ViewModels.Cart
{
    public record CartViewModel
    {
        public int ID { get; set; }

        public string UserID { get; set; }

        public virtual ICollection<CartItemsViewModel> CartItems { get; set; } = new List<CartItemsViewModel>();
    }
}
