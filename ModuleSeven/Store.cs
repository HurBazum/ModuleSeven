using DeleteLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModuleSeven
{
    internal class Store
    {
        //переменные для создания заказа
        public string enterProduct = "";
        public string enterAddress = "";
        public string enterNum = "";
        public bool WannaChangeHomeDeliveryDate = default;
        public List<Product> Products { get; set; }
        public List<Order<Delivery, int>> OrdersIntId { get; set; }
        public List<Order<Delivery, string>> OrdersStringId { get; set; }
        public List<Customer> Customers { get; set; }

        public Store()
        {
            Products = new List<Product>();
            OrdersIntId = new List<Order<Delivery, int>>();
            OrdersStringId = new List<Order<Delivery, string>>();
            Customers = new List<Customer>();
        }

        public void FillProductsList()
        {
            Random For_Products = new();
            int AmountAvailableProducts = For_Products.Next(10, 20);//генерируется количество продуктов

            //заполнение списка продуктов
            for (int i = 0; i < AmountAvailableProducts; i++)
            {
                if (For_Products.Next(0, 2) == 0)//создается продукт со скидкой
                    {
                        Products.Add(new(Math.Round(For_Products.NextDouble(), 2)
                           + For_Products.Next(100, 10000), For_Products.Next(1, 21), For_Products.Next(10, 30)));
                }
                else//без скидки
                {
                    Products.Add(new(Math.Round(For_Products.NextDouble(), 2)
                        + For_Products.Next(100, 10000), For_Products.Next(1, 21)));
                }
            }
        }
        public void ShowProductsList()
        {
            foreach(var product in Products)
            {
                Console.WriteLine(product);
            }
        }
        //public Order(object orderId, int chosenDelivery, string address, int numberProducts, ref Product product)
        public void CreateOrder(Customer customer, string enterProduct, string enterNum, 
            string enterAddress, bool WannaChangeHomeDeliveryDate)
        {
            Console.WriteLine("Итак, чтобы совершить заказ, нужно ответить на несколько вопросов!");
            Console.Write("Введите название товара: ");

            do
            {
                DltSmthg._setCursorPosition(out int x, out int y);
                enterProduct = Console.ReadLine();
                enterProduct = enterProduct.ToUpper();
                var gottenProduct = Products.Find(x => x.Name == enterProduct);//поиск товара с введенным названием в Products,
                                                                               //если такого нет - возвращает null
                if (gottenProduct != null)
                {

                    EnterNumberProducts(out int count, enterProduct, enterNum);//запрос ввода кол-ва продуктов в заказ, кол-во должно быть 
                                                                                         //отличным от ноля и не больше, чем имеется в магазине

                    EnterNumber(out int choice, enterNum);//выбор способа доставки

                    switch (choice)//создание экземпляра класса доставки, с предварительным вводом адреса
                    {
                        case 1:
                            OrdersIntId.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, count, ref gottenProduct));
                            customer.MyIntOrders.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, count, ref gottenProduct));
                            customer.MyBills.Add(new Bill(customer.MyIntOrders[customer.MyIntOrders.Count - 1].ToString()));
                            break;

                        case 2:
                            OrdersIntId.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, count, ref gottenProduct));
                            customer.MyIntOrders.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, count, ref gottenProduct));
                            customer.MyBills.Add(new Bill(customer.MyIntOrders[customer.MyIntOrders.Count - 1].ToString()));
                            break;
                        case 3:
                            EnterAddress(ref enterAddress);//выбор места доставки
                            OrdersIntId.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, count, ref gottenProduct));
                            customer.MyIntOrders.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, count, ref gottenProduct));
                            customer.MyBills.Add(new Bill(customer.MyIntOrders[customer.MyIntOrders.Count - 1].ToString()));
                            Console.WriteLine($"Текущая дата доставки - {OrdersIntId[OrdersIntId.Count - 1].Delivery.DeliveryDate.ToShortDateString()}," +
                            $" хотите ли её изменить?(да\\нет)");
                            EnterYesOrNo(ref WannaChangeHomeDeliveryDate);
                            if (WannaChangeHomeDeliveryDate == true)
                            {
                                OrdersIntId[OrdersIntId.Count - 1].Delivery.DeliveryDate = EnterDate(out DateTime date, OrdersIntId[OrdersIntId.Count - 1].Delivery);
                                customer.MyIntOrders[customer.MyIntOrders.Count - 1].Delivery.DeliveryDate = OrdersIntId[OrdersIntId.Count - 1].Delivery.DeliveryDate; 
                                WannaChangeHomeDeliveryDate = false;
                            }
                            else
                            {
                                continue;
                            }
                            break;
                    }
                }
                else
                {
                    DltSmthg.deleteWrongEnter(x, y, enterProduct);
                    enterProduct = null;
                }
            } while (enterProduct == null);
        }
        public void EnterNumber(out int choice, string enterNum)
        {
            Console.WriteLine("Выберете способ доставки:\n1.В магазин\n2.В пункт доставки\n3.На дом");
            DltSmthg._setCursorPosition(out int x, out int y);
            choice = 0;
            do
            {
                enterNum = Console.ReadLine();
                if (!int.TryParse(enterNum, out choice) || (choice > 3 || choice < 1))
                {
                    DltSmthg.deleteWrongEnter(x, y, enterNum);
                    choice = 0;
                }
            } while (choice == 0);
            enterNum = "";
        }
        public void EnterNumberProducts(out int count, string enterProduct, string enterNum)
        {
            Console.Write("Введите количество товара, которое хотите заказать: ");
            DltSmthg._setCursorPosition(out int x, out int y);
            count = 0;
            do
            {
                enterNum = Console.ReadLine();
                if (!int.TryParse(enterNum, out count) || !((Products.Find(x => x.Name == enterProduct).Count >= count)))
                {
                    DltSmthg.deleteWrongEnter(x, y, enterNum);
                    count = 0;
                }
            } while (count == 0);
            enterNum = "";
        }
        public void EnterYesOrNo(ref bool answer)
        {

            DltSmthg._setCursorPosition(out int x, out int y);
            string enter;
            do
            {
                enter = Console.ReadLine();
                if (!enter.Equals("да", StringComparison.CurrentCultureIgnoreCase)
                    && !enter.Equals("нет", StringComparison.CurrentCultureIgnoreCase))
                {
                    DltSmthg.deleteWrongEnter(x, y, enter);
                    enter = null;
                }
            } while (enter == null);
            if (enter.Equals("да", StringComparison.CurrentCultureIgnoreCase))
            {
                answer = true;
            }
            else
            {
                answer = false;
            }
        }
        public void EnterAddress(ref string address)
        {

            Console.Write("Теперь введите адрес(улица, номер дома): ");
            do
            {
                DltSmthg._setCursorPosition(out int third_x, out int third_y);

                address = Console.ReadLine();
                if (!Regex.IsMatch(address, @"\b\w\d{1,3}\w{0,1}\b"))
                {
                    DltSmthg.deleteWrongEnter(third_x, third_y, address);
                    address = null;
                }
            } while (address == null);
        }
        public DateTime EnterDate<T>(out DateTime date, T order) where T : Delivery
        {
            date = order.DeliveryDate;
            if (order is HomeDelivery)
            {
                string enterDate = "";

                Console.WriteLine(order.deliveryDate.ToShortDateString());
                Console.WriteLine("Введите новую дату, позднее текущей: ");

                do
                {
                    DltSmthg._setCursorPosition(out int x, out int y);
                    enterDate = Console.ReadLine();
                    if (!DateTime.TryParse(enterDate, out date) || date <= order.DeliveryDate)
                    {
                        DltSmthg.deleteWrongEnter(x, y, enterDate);
                        date = order.DeliveryDate;
                    }
                } while (date == order.DeliveryDate);
            }
            return date;
        }
    }
}
