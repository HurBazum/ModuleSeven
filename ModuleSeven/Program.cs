using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using DeleteLib;

namespace ModuleSeven
{
    //статус заказа
    enum OrderStatus
    {
        None = 0,
        Build,
        OnItsWay,
        CanGet
    }
    public class Program
    {
        static void Main()
        {
            //переменные для создания заказа
            string enterProduct = "";
            string enterAddress = "";
            Delivery delivery = null;
            string enterNum = "";
            Order<Delivery, int> order = null;
            bool WannaChangeHomeDeliveryDate = default;
            Store store = new();
            //создание ассортимента
            store.FillProductsList();
            //вывод ассортимента на экран
            store.ShowProductsList();
            //добавляем покупателя
            store.Customers.Add(new Customer("Anton", 30000, "923337358"));
            ////*******************************************************************************
            ///создаем заказ, он добавляется и в магазин, и покупателю, также у покупателя появляется чек
            ///с статусом "Не оплачено" + order.ToString()
            store.CreateOrder(store.Customers[0], enterProduct, enterNum, enterAddress, WannaChangeHomeDeliveryDate);
            Console.WriteLine(store.OrdersIntId[0].ToString());
            Console.WriteLine(store.OrdersIntId[0].Description);
            store.OrdersIntId[0].DisplayAddress();
            //оплачиваем заказ, т.е. меняем "Не оплачено" на "оплачено", также статус заказа с None на Build
            //
            store.Customers[0].PayTheBill(store.Customers[0].MyIntOrders[0].Id);
            Console.WriteLine(store.Customers[0].ToString());
            Console.WriteLine(store.Customers[0].MyBills[store.Customers[0].MyBills.Count - 1].ToString());
            //изменяем информацию в чеке и в заказе, а точнее OrderStatus на OnItsWay или CanGet
            store.GiveOrderToDeliveryOrSayItDelivered(store.Customers[0].MyIntOrders[0]);
            Console.WriteLine(store.Customers[0].MyBills[store.Customers[0].MyBills.Count - 1].ToString());
            Console.WriteLine(store.Customers[0].MyIntOrders[0].ToString());
        }
    }
}
