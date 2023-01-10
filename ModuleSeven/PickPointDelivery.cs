namespace ModuleSeven
{
    class PickPointDelivery : Delivery
    {
        public PickPointDelivery(string address, ref Product product)
            : base(address, ref product)
        {
            deliveryType = "Доставка в пункт выдачи";
            deliveryDate = DateTime.Now.AddDays(15);
            Price = (product.Price > 2000) ? 0 : 200;
        }
        public PickPointDelivery()
        {

        }
        public void CheckDeliveryPrice()
        {
            Price = (Product.Price > 2000) ? 0 : 2000;
        }
            
    }
}
