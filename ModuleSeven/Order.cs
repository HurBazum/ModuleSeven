namespace ModuleSeven
{
    class Order<TDelivery, TStruct> where TDelivery : Delivery
    {
        public TDelivery Delivery;
        public TStruct Id { get; private set; }
        public int NumberProducts { get; set; }//кол-во заказываемого товара
        public string Description;//название продукта_заказанное количество
        public OrderStatus OrderStatus;//статус заказа
        public string OrderNumber;//ZK_#_DATE
        
        private double price;

        //если количество товара в наличии меньше, чем пользователь хочет заказать,
        //то в заказ включается только то, что есть
        public Order(object orderId, int chosenDelivery, string address, int numberProducts, ref Product product)
        {
            Id = (TStruct)orderId;
            switch(chosenDelivery)
            {
                case 1:
                    Delivery = (TDelivery)Activator.CreateInstance(typeof(ShopDelivery));
                    break;
                case 2:
                    Delivery = (TDelivery)Activator.CreateInstance(typeof(PickPointDelivery));
                    break;
                case 3:
                    Delivery = (TDelivery)Activator.CreateInstance(typeof(HomeDelivery));
                    break;
            }
            //Delivery = delivery;
            Delivery.Product = product;
            NumberProducts = numberProducts;
            Delivery.Address = address;
            
            Description = $"{Delivery.Product.Name}_{NumberProducts}";
            //расчёт цены заказа в зависимости от наличия скидки и вида доставки
            if(Delivery is PickPointDelivery)
            {
                (Delivery as PickPointDelivery).CheckDeliveryPrice();
            }
            price = (Delivery.Product.Discount != 0)
                ? Delivery.Product.Price * ((1 - (Delivery.Product.Discount / 100.00)) * NumberProducts) + 
                    Delivery.Price : (Delivery.Product.Price * NumberProducts) + Delivery.Price;
            //заказ поступил в работу
            OrderStatus = OrderStatus.Build;
            product.IsOrdered = true;
            product.ChangeCount(numberProducts);

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
                Delivery.DeliveryDate = NewDeliveryDate;
            }
        }
        public override string ToString()
        {
            return $"ID Заказа - {Id}\n" +
                $"Вы заказали {NumberProducts} {Delivery.Product.Name} " +
                $"на сумму {price:C2}.\nСтатус заказа - {OrderStatus}.\n" +
                $"Вид доставки {Delivery.DeliveryType}, доставка {Delivery.DeliveryDate.ToShortDateString()}.";
        }
    }
}
