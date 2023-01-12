namespace ModuleSeven
{
    class ShopDelivery : Delivery
    {
        public ShopDelivery()
        {
            deliveryType = "Доставка на магазин";
            DeliversDays = 30;//дни в течении которых товар будет доставляться
            DeliveryDate = DateTime.Now.AddDays(DeliversDays);//устанавливается дата, когда товар можно будет забрать
            ShopAddress = new string[] { "Партизанская, 11б", "Ярославская, 4" };
            Address = ShopAddress[0];
        }
        /// <summary>
        /// выводит адреса доступных магазинов
        /// </summary>
        public override void ShowAvailableAdress()
        {
            for (int i = 0; i < ShopAddress.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {ShopAddress[i]}");
            }
        }
    }
}
