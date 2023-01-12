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
        None = 0,//заказ создан
        Build,//оплачен
        OnItsWay,//доставляется
        CanGet,//можно забрать
        Gotten//получен
    }
    public class Program
    {
        static void Main()
        {
            //случайный выбор TStruct в Order<Tdelivery, TStruct>
            Random randomId = new Random();
            object[] objects = { 1, "string" };//int, string
            object id = randomId.Next(0, 2) == 0 ? objects[0] : objects[1];
            Console.WriteLine(id);
            //создание магазина
            Store store = new();
            //создание ассортимента
            store.FillProductsList();
            //вывод ассортимента на экран
            store.ShowProductsList();
            //добавляем покупателя c корректным номером и возжностью оплатить заказ
            store.Customers.Add(new Customer("Anton", 30000, "9233397358", "Пролетарская, 12"));
            ////*******************************************************************************
            ///создаем заказ, он добавляется и в магазин, и покупателю, также у покупателя появляется чек
            ///с статусом "Не оплачено" + order.ToString()
            Console.WriteLine("\nПокупатель и его счёт:");
            Console.WriteLine(store.Customers[0].ToString());
            Console.WriteLine();
            store.CreateOrder(id, store.Customers[0], store.enterProduct, store.enterNum, store.enterAddress, store.WannaChangeHomeDeliveryDate);
            if (id is int)
            {
                Console.WriteLine("вывод информации по заказу со статусом None\n");
                Console.WriteLine(store.Customers[0].MyBills[store.Customers[0].MyBills.Count - 1].ToString());
                Console.WriteLine("\nоплачиваем заказ, т.е. меняем \"Не оплачено\" на \"оплачено\", также статус заказа с None на Build\n");
                store.Customers[0].PayTheBill(store.Customers[0].MyIntOrders[0].Id);
                Console.WriteLine("выводим оплаченный чек\n");
                Console.WriteLine(store.Customers[0].MyBills[store.Customers[0].MyBills.Count - 1].ToString());
                Console.WriteLine("\nсмотрим изменения customer(состояние счёта), после удачной оплаты\n");
                Console.WriteLine(store.Customers[0].ToString());
                Console.WriteLine("\nизменяем информацию в чеке и в заказе, а точнее OrderStatus на OnItsWay или CanGet\n");
                store.GiveOrderToDeliveryOrSayItDelivered(store.Customers[0].MyIntOrders[0]);
                Console.WriteLine("опять смотрим изменения в чеке\n");
                Console.WriteLine(store.Customers[0].MyBills[store.Customers[0].MyBills.Count - 1].ToString());
                Console.WriteLine();
            }
            //тоже самое с заказом со строковым ID
            if(id is string && !String.IsNullOrWhiteSpace((id as string)))
            {
                Console.WriteLine("вывод информации по заказу со статусом None\n");
                Console.WriteLine(store.Customers[0].MyBills[store.Customers[0].MyBills.Count - 1].ToString());
                Console.WriteLine("\nоплачиваем заказ, т.е. меняем \"Не оплачено\" на \"оплачено\", также статус заказа с None на Build\n");
                store.Customers[0].PayTheBill(store.Customers[0].MyStringOrders[0].Id);
                Console.WriteLine("выводим оплаченный чек\n");
                Console.WriteLine(store.Customers[0].MyBills[store.Customers[0].MyBills.Count - 1].ToString());
                Console.WriteLine("\nсмотрим изменения customer(состояние счёта), после удачной оплаты\n");
                Console.WriteLine(store.Customers[0].ToString());
                Console.WriteLine("\nизменяем информацию в чеке и в заказе, а точнее OrderStatus на OnItsWay или CanGet\n");
                store.GiveOrderToDeliveryOrSayItDelivered(store.Customers[0].MyStringOrders[0]);
                Console.WriteLine("опять смотрим изменения в чеке\n");
                Console.WriteLine(store.Customers[0].MyBills[store.Customers[0].MyBills.Count - 1].ToString());
                Console.WriteLine();
            }
            //вывод обновлённого списка товаров, после оплаты заказа
            store.ShowProductsList();
        }
    }
}
