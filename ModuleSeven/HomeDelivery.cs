namespace ModuleSeven
{
    class HomeDelivery : Delivery
    {
        private int MinDaysForDelivery = 10;//минимальное количество дней для доставки, нужно для ограничения изменения даты доставки
        /// <summary>
        /// для возможности перенесения даты доставки по договорённости с курьером,
        /// но пока курьера нет . . .
        /// </summary>
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
        public HomeDelivery()
        {
            deliveryType = "Доставка на дом";
            deliveryDate = DateTime.Now.AddDays(MinDaysForDelivery);
            Price = 500;
        }
        /// <summary>
        /// изменение даты доставки, через метод
        /// </summary>
        /// <param name="NewDate"></param>
        public void ChangeDeliveryDate(DateTime NewDate)
        {
            DeliveryDate = NewDate;
        }
    }
}
