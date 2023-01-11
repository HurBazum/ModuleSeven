namespace ModuleSeven
{
    class HomeDelivery : Delivery
    {
        private int MinDaysForDelivery = 10;
        public override DateTime DeliveryDate
        {
            get
            {
                return deliveryDate;
            }
            set
            {
                if(value >= DateTime.Now.AddDays(MinDaysForDelivery))
                {
                    deliveryDate = value;
                }
                else
                {
                    deliveryDate = DateTime.Now.AddDays(MinDaysForDelivery);
                }
            }
        }
        
        public HomeDelivery(string address, ref Product product)
            : base(address, ref product)
        {
            deliveryType = "Доставка на дом";
            deliveryDate = DateTime.Now.AddDays(MinDaysForDelivery);
            Price = 500;
        }
        public HomeDelivery()
        {
            deliveryType = "Доставка на дом";
            deliveryDate = DateTime.Now.AddDays(MinDaysForDelivery);
            Price = 500;
        }
        public void ChangeDeliveryDate(DateTime NewDate)
        {
            DeliveryDate = NewDate;
        }
    }
}
