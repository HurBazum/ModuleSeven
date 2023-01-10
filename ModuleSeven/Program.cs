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
        Build = 0,
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
            
            ////*******************************************************************************
            store.CreateOrder(enterProduct, enterNum, enterAddress, WannaChangeHomeDeliveryDate);
            Console.WriteLine(store.OrdersIntId[0].ToString());
            Console.WriteLine(store.OrdersIntId[0].Description);
            store.OrdersIntId[0].DisplayAddress();
            foreach (Product product in store.Products)
            {
                Console.WriteLine(product);
            }
        }
    }
}
