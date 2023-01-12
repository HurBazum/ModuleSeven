using System;

namespace ModuleSeven
{
    abstract class Delivery
    {
        public virtual string Address { get; set; }
        public string deliveryType;//тип доставки, используется в Bill.ToString();
        public int DeliversDays = 0;//сколько товар будет доставлятся
        public string DeliveryType { get { return deliveryType; } }
        public double Price;
        public DateTime deliveryDate;
        public int DaysForDeliver;//сколько осталось дней до доставки товара
        public virtual DateTime DeliveryDate { get; set; }//переопределяется в HomeDelivery
        public Product Product { get; set; }
        public string[] ShopAddress { get; set; }//возможные адреса магазинов
        public string[] PickPointAdress { get; set; }//возможные адреса пунктов выдачи

        public Delivery()
        {

        }
        //присваивает DaysForDeliver количество оставшихся дней до доставки
        //и возвращает сообщение об оставшихся днях. используется для Order.ToString();
        public string DaysForDelivery()
        {
            ///////???????????
            if (DateTime.Now.Month < deliveryDate.Month)//если доставка планируется в следующем
            {
                DaysForDeliver = DateTime.Now.Day - deliveryDate.Day + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            }
            else//если доставка планируется в текущем месяце
            {
                DaysForDeliver = DateTime.Now.Day - deliveryDate.Day;
            }
            return $"Заказ доставят через {DaysForDeliver} дней.";
        }
        public virtual void ShowAvailableAdress()//Переопределяется в ShopDelivery и PickPointDelivery 
        {
            Console.WriteLine(Address);
        }
        public override string ToString()
        {
            return $"{DeliveryType}\n{Address}\n{DeliveryDate}\n{Product.ToString()}";
        }
    }
}
