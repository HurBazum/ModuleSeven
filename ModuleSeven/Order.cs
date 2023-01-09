namespace ModuleSeven
{
    class Order<TDelivery, TStruct> where TDelivery : Delivery
    {
        public TDelivery Delivery;
        public int NumberProducts { get; set; }//кол-во заказываемого товара
        public string Description;//название продукта_заказанное количество
        public OrderStatus OrderStatus;//статус заказа
        public string OrderNumber;//ZK_#_DATE
        
        private double price;

        //если количество товара в наличии меньше, чем пользователь хочет заказать,
        //то в заказ включается только то, что есть
        public Order(TDelivery delivery, int number)
        {
            Delivery = delivery;
            NumberProducts = number;
            Description = $"{delivery.Product.Name}_{NumberProducts}";
            //расчёт цены заказа в зависимости от наличия скидки
            price = (Delivery.Product.Discount != 0)
                ? Delivery.Product.Price * ((1 - (Delivery.Product.Discount / 100.00)) * NumberProducts) + 
                    Delivery.Price : (Delivery.Product.Price * NumberProducts) + Delivery.Price;
            //заказ поступил в работу
            OrderStatus = OrderStatus.Build;
        }

        public void DisplayAddress()
        {
            Console.WriteLine(Delivery.Address);
        }
        //измение статуса заказа
        public void ChangeDeliveryStatus()
        {
            if (OrderStatus != OrderStatus.CanGet)
            {
                OrderStatus++;
            }
        }
        public void ChangeDate(DateTime NewDeliveryDate)
        {
            if (Delivery is HomeDelivery)
            {
                Delivery.deliveryDate = NewDeliveryDate;
            }
        }
        public override string ToString()
        {
            return $"Вы заказали {NumberProducts} {Delivery.Product.Name} " +
                $"на сумму {price:C2}. Статус заказа - {OrderStatus}.";
        }
    }
}
