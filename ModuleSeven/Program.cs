using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
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
    abstract class Delivery
    {
        public string Address;
        public double Price;
        public DateTime DeliveryDate;
        public int DaysForDeliver;
        public Product Product;
        public Delivery(string address, ref Product product)//Product передаётся по ссылке, чтобы изменить его переменную IsOrder,
                                                            //чтобы при удачном заказе, уменьшилось его кол-во в магазине
        {
            Address = address;
            Product = product;
            product.IsOrdered = true;
        }
        public override string ToString()
        {
            return $"Адрес - {Address}\nСтоимость - {Price}";
        }
    }
    class HomeDelivery : Delivery
    {
        public HomeDelivery(string address, ref Product product)
            : base(address, ref product) 
        { 
            Price = 500;
        }
    }

    class PickPointDelivery : Delivery
    {
        public PickPointDelivery(string address, ref Product product)
            : base(address, ref product) 
        { 
            Price = 200;
        }
    }

    class ShopDelivery : Delivery
    {
        public ShopDelivery(string address, ref Product product)
            : base(address, ref product) 
        { 
            Price = 0; 
        }
    }
    class Program
    {
        static void Main()
        {
            //переменные для создания заказа
            string enterProduct = "";
            string enterAddress = "";
            Delivery delivery = null;
            string enterNum = "";
            Order<Delivery, int> order = null;

            //создание ассортимента
            Random For_Products = new();
            int AmountAvailableProducts = For_Products.Next(10, 20);//генерируется количество продуктов
            List<Product> Products = new List<Product>();
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
            //вывод ассортимента на экран
            foreach (Product product in Products)
            {
                Console.WriteLine(product);
            }
            //********************************************************************************

            CreateOrder(ref order, Products, delivery, enterProduct, enterNum, enterAddress);
            Console.WriteLine(order.ToString());
            Console.WriteLine(order.Description);

        }
        //методы для заполнения полей классов, с проверкой корректного ввода 
        //
        static void CreateOrder(ref Order<Delivery, int> order, List<Product> Products, 
            Delivery delivery, string enterProduct, string enterNum, string enterAddress)
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

                    EnterNumberProducts(out int count, Products, enterProduct, enterNum);//запрос ввода кол-ва продуктов в заказ, кол-во должно быть 
                                                                                         //отличным от ноля и не больше, чем имеется в магазине
                    EnterNumber(out int choice, enterNum);//выбор места доставки

                    switch (choice)//создание экземпляра класса доставки, с предварительным вводом адреса
                    {
                        case 1:
                            EnterAddress(ref enterAddress);
                            delivery = new ShopDelivery(enterAddress, ref gottenProduct);
                            break;

                        case 2:
                            EnterAddress(ref enterAddress);
                            delivery = new PickPointDelivery(enterAddress, ref gottenProduct);
                            break;

                        case 3:
                            EnterAddress(ref enterAddress);
                            delivery = new HomeDelivery(enterAddress, ref gottenProduct);
                            break;
                    }

                    order = new(delivery, count);
                    //уменьшаем количество данного товара на количество в заказе,
                    //после меняем значение Product.IsOrdered на false,
                    //чтобы можно было увелить количество товара
                    //с помощью того же метода ChangeCount
                    gottenProduct.ChangeCount(order.NumberProducts);
                    gottenProduct.IsOrdered = default;
                }
                else
                {
                    DltSmthg.deleteWrongEnter(x, y, enterProduct);
                    enterProduct = null;
                }
            } while (order == null);
        }
        static void EnterNumber(out int choice, string enterNum)
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
        static void EnterNumberProducts(out int count, List<Product> Products, string enterProduct, string enterNum)
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
        static void EnterAddress(ref string address)
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
    }
}
