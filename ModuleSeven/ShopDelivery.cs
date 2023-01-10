namespace ModuleSeven
{
    class ShopDelivery : Delivery
    {
        public ShopDelivery()
        {
            deliveryType = "Доставка на магазин";
            deliveryDate = DateTime.Now.AddDays(30);
        }
    }
}
