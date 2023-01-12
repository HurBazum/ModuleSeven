namespace ModuleSeven
{
    class PickPointDelivery : Delivery
    {
        public PickPointDelivery()
        {
            deliveryType = "Доставка в пункт выдачи";
            DeliversDays = 15;
            DeliveryDate = DateTime.Now.AddDays(DeliversDays);//устанавливается дата, когда товар можно будет забрать
            PickPointAdress = new string[2] { "Фрунзе, 2", "Ленина, 111" };
            Address = PickPointAdress[0];
        }
        /// <summary>
        /// расчитывает цену доставки, в зависимости от цены товара
        /// </summary>
        public void CheckDeliveryPrice()
        {
            Price = (Product.Price > 2000) ? 0 : 200;
        }
        /// <summary>
        /// выводит адреса доступных пунктов выдачи
        /// </summary>
        public override void ShowAvailableAdress()
        {
            for (int i = 0; i < PickPointAdress.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {PickPointAdress[i]}");
            }
        }
    }
}
