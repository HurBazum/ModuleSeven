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
        public string enterNumShop = "";
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
        //создание заказа, с помощью ввода необходимых значений
        //
        public void CreateOrder(object chosenId, Customer customer, string enterProduct, string enterNum, 
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

                    Console.WriteLine("Выберете способ доставки:\n1.В магазин\n2.В пункт доставки\n3.На дом");
                    EnterNumber(out int choice, enterNum);//выбор способа доставки

                    switch (choice)//создание экземпляра класса доставки, с предварительным вводом адреса
                    {
                        //доставка на магазин
                        case 1:
                            if (chosenId is int)
                            {
                                OrdersIntId.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, 
                                    count, ref gottenProduct, customer.Name));
                                //выводятся значения возможных адресов доставки
                                Console.WriteLine("Выберете адрес магазина, на который вам нужна доставка: ");
                                OrdersIntId[OrdersIntId.Count - 1].Delivery.ShowAvailableAdress();
                                var d = OrdersIntId[OrdersIntId.Count - 1].Delivery;
                                //выбирается один из адресов, в последствии адрес доставки в заказе меняется 
                                //на выбранный
                                EnterNumber(out int choiceAddress, enterNumShop, 1, d.ShopAddress.Length);
                                d.Address = d.ShopAddress[choiceAddress - 1];//присваиваем выбранный адрес полю Delivery.Address
                                //***********************************************************************************
                                customer.MyIntOrders.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, d.Address, 
                                    count, ref gottenProduct, customer.Name));
                                //***********************************************************************************
                                customer.MyBills.Add(new Bill(customer.MyIntOrders[customer.MyIntOrders.Count - 1].ToString()));
                            }
                            if (chosenId is string)
                            {
                                OrdersStringId.Add(new Order<Delivery, string>("Shop" + OrdersIntId.Count.ToString(), 
                                    choice, enterAddress, count, ref gottenProduct, customer.Name));
                                Console.WriteLine("Выберете адрес магазина, на который вам нужна доставка: ");
                                OrdersStringId[OrdersStringId.Count - 1].Delivery.ShowAvailableAdress();
                                var d = OrdersStringId[OrdersStringId.Count - 1].Delivery;
                                EnterNumber(out int choiceAddress, enterNumShop, 1, d.ShopAddress.Length);
                                d.Address = d.ShopAddress[choiceAddress - 1];
                                //***********************************************************************************
                                customer.MyStringOrders.Add(new Order<Delivery, string>("Shop" + OrdersIntId.Count.ToString(), 
                                    choice, d.Address, count, ref gottenProduct, customer.Name));
                                //***********************************************************************************
                                customer.MyBills.Add(new Bill(customer.MyStringOrders[customer.MyStringOrders.Count - 1].ToString()));
                            }
                        break;
                            //доставка в пункт выдачи
                        case 2:
                            if (chosenId is int)
                            {
                                //те же действия, что и при выборе доставки на магазин
                                OrdersIntId.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, 
                                    count, ref gottenProduct, customer.Name));

                                Console.WriteLine("Выберете адрес пункта выдачи, на который вам нужна доставка: ");

                                OrdersIntId[OrdersIntId.Count - 1].Delivery.ShowAvailableAdress();
                                var d = OrdersIntId[OrdersIntId.Count - 1].Delivery;

                                EnterNumber(out int choiceAddress, enterNumShop, 1, d.PickPointAdress.Length);
                                d.Address = d.PickPointAdress[choiceAddress - 1];

                                customer.MyIntOrders.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, d.Address, 
                                    count, ref gottenProduct, customer.Name));

                                customer.MyBills.Add(new Bill(customer.MyIntOrders[customer.MyIntOrders.Count - 1].ToString()));
                            }
                            if (chosenId is string)
                            {
                                OrdersStringId.Add(new Order<Delivery, string>("PickPoint" + OrdersStringId.Count.ToString(), choice, enterAddress,
                                    count, ref gottenProduct, customer.Name));

                                Console.WriteLine("Выберете адрес пункта выдачи, на который вам нужна доставка: ");

                                OrdersIntId[OrdersIntId.Count - 1].Delivery.ShowAvailableAdress();
                                var d = OrdersIntId[OrdersIntId.Count - 1].Delivery;

                                EnterNumber(out int choiceAddress, enterNumShop, 1, d.PickPointAdress.Length);
                                d.Address = d.PickPointAdress[choiceAddress - 1];


                                customer.MyStringOrders.Add(new Order<Delivery, string>("PickPoint" + OrdersStringId.Count.ToString(), choice, 
                                    enterAddress, count, ref gottenProduct, customer.Name));

                                customer.MyBills.Add(new Bill(customer.MyStringOrders[customer.MyStringOrders.Count - 1].ToString()));
                            }
                        break;
                            //доставка на дом
                        case 3:
                            if (customer.Address == null)
                            {
                                EnterAddress(ref enterAddress);//ввод домашнего адреса получателя, если он не заполнен
                            }
                            else
                            {
                                enterAddress = customer.Address;
                            }
                            if (chosenId is int)
                            {
                                OrdersIntId.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, 
                                    count, ref gottenProduct, customer.Name));

                                customer.MyIntOrders.Add(new Order<Delivery, int>(OrdersIntId.Count, choice, enterAddress, 
                                    count, ref gottenProduct, customer.Name));

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
                            }
                            if(chosenId is string)
                            {
                                OrdersStringId.Add(new Order<Delivery, string>("Home" + OrdersStringId.Count.ToString(), 
                                    choice, enterAddress, count, ref gottenProduct, customer.Name));

                                customer.MyStringOrders.Add(new Order<Delivery, string>("Home" + OrdersStringId.Count.ToString(), 
                                    choice, enterAddress, count, ref gottenProduct, customer.Name));

                                customer.MyBills.Add(new Bill(customer.MyStringOrders[customer.MyStringOrders.Count - 1].ToString()));

                                Console.WriteLine($"Текущая дата доставки - " +
                                    $"{OrdersIntId[OrdersIntId.Count - 1].Delivery.DeliveryDate.ToShortDateString()}," +
                                        $" хотите ли её изменить?(да\\нет)");
                                EnterYesOrNo(ref WannaChangeHomeDeliveryDate);
                                //корректировка даты доставки на дом
                                if (WannaChangeHomeDeliveryDate == true)
                                {
                                    OrdersIntId[OrdersIntId.Count - 1].Delivery.DeliveryDate = EnterDate(out DateTime date, OrdersIntId[OrdersIntId.Count - 1].Delivery);
                                    customer.MyIntOrders[customer.MyIntOrders.Count - 1].Delivery.DeliveryDate = OrdersIntId[OrdersIntId.Count - 1].Delivery.DeliveryDate;
                                    WannaChangeHomeDeliveryDate = false;
                                }
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
            enterProduct = "";
            enterAddress = "";
            enterNum = "";
            WannaChangeHomeDeliveryDate = default;
        }
        //возвращает число, находящееся в заданных границах, используется для switch() и 
        //для выбора элемента из массивов ShopAddress/PickPointAdress
        public void EnterNumber(out int choice, string enterNum, int MinValue = 1, int MaxValue = 3)
        {
            DltSmthg._setCursorPosition(out int x, out int y);
            choice = 0;
            do
            {
                enterNum = Console.ReadLine();
                if (!int.TryParse(enterNum, out choice) || (choice > MaxValue || choice < MinValue))
                {
                    DltSmthg.deleteWrongEnter(x, y, enterNum);
                    choice = 0;
                }
            } while (choice == 0);
            enterNum = "";
        }
        //задаёт количество товара для заказа
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
        //получает ответ на вопрос да или нет
        //используется для корректировки даты заказа на дом, при его оформлении
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
        //корректный ввод адреса, если у покупателя он не указан
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
        //измение даты заказа, если используется HomeDelivery
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

        //ищет покупателя с данным заказом и изменяет статус заказа в счёте и в заказе
        //при этом заказы со статусом None, т.е. не оплаченные не обрабатываются
        //по идее сюда ещё должен передаваться класс доставщика(транспортная компания или курьер),
        //чтобы присваивать при передаче им заказа статус OrderStatus.OnItsWay
        //и если у них поле e.g. Isdelivered == true, то можно присвоить OrderStatus.CanGet
        //но этих классов пока нет, поэтому статус заказа не изменяется, только если у клиента
        //недостаточно средств на счету.
        public void GiveOrderToDeliveryOrSayItDelivered<T>(Order<Delivery, T> order)
        {
            if (order.OrderStatus != OrderStatus.None)
            {
                string customerName = order.CustomerName;
                var rightCustomer = Customers.Find(x => x.Name == customerName);
                if (order is Order<Delivery, int>)
                {
                    var rightCustomerOrder = rightCustomer.MyIntOrders.Find(x => x.Description == order.Description);
                    string search = rightCustomerOrder.ToString();
                    var changedBill = rightCustomer.MyBills.Find(x => x.BillStatus.Contains(search));
                    changedBill.ChangeBillInfo(rightCustomer, ref rightCustomerOrder);
                }
                if (order is Order<Delivery, string>)
                {
                    var rightCustomerOrder = rightCustomer.MyStringOrders.Find(x => x.Description == order.Description);
                    string search = rightCustomerOrder.ToString();
                    var changedBill = rightCustomer.MyBills.Find(x => x.BillStatus.Contains(search));
                    changedBill.ChangeBillInfo(rightCustomer, ref rightCustomerOrder);
                }
            }
            else
            {
                Console.WriteLine("Операция не возможна!");
            }
        }
    }
}
