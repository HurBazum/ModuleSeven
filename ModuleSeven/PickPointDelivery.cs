namespace ModuleSeven
{
    class PickPointDelivery : Delivery
    {
        public PickPointDelivery()
        {

        }
        public void CheckDeliveryPrice()
        {
            DeliveryDate = DateTime.Now.AddDays(15);
            Price = (Product.Price > 2000) ? 0 : 200;
        }
            
    }
}
