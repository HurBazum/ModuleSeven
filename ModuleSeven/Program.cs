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
            Customer customer = new Customer("Anton", 30000, "9233397358");
            store.Customers.Add(customer);
            ////*******************************************************************************
            store.CreateOrder(store.Customers[0], enterProduct, enterNum, enterAddress, WannaChangeHomeDeliveryDate);
            Console.WriteLine(store.OrdersIntId[0].ToString());
            Console.WriteLine(store.OrdersIntId[0].Description);
            store.OrdersIntId[0].DisplayAddress();
            store.Customers[0].PayTheBill(0);
            Console.WriteLine(store.Customers[0].ToString());
        }
    }
}
