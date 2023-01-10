namespace ModuleSeven
{
    class ShopDelivery : Delivery
    {
        public ShopDelivery(string address, ref Product product)
            : base(address, ref product)
        {
            deliveryType = "Доставка на магазин";
            deliveryDate = DateTime.Now.AddDays(30);
        }
    }
}
