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
    class Product
    {
        //имена продукции создаются рандомно
        Random ProductRandomName = new Random();
        string name;
        public string Name
        {
            get { return name; }
            private set
            {
                value = "";
                int NameLength = ProductRandomName.Next(3, 10);
                for (int i = 0; i < NameLength; i++)
                {
                    value += (char)ProductRandomName.Next(65, 91);
                }
                name = value;
            }
        }
        public double Price { get; set; }
        public int Count { get; set; }
        public double Discount;
        public Product(double price, int count, double discount = default)
        {
            Name = Name;
            Price = price;
            Count = count;
            Discount = discount;
        }
        public void ChangeCount(int count)
        {
            Count += count;
        }
        public override string ToString()
        {
            return (Discount != default) 
                ? $"{Name,9} стоит {Price, 3}, скидка = {Discount}%, в наличии {Count}"
                    : $"{Name,9} стоит {Price, 3}, скидок нет, в наличии {Count}";
        }
    }
    abstract class Delivery
    {
        public string Address;
        public double Price;
        public DateTime DeliveryDate;
        public int DaysForDeliver;
        public Product Product;
        public Delivery(string address, Product product)
        {
            Address = address;
            Product = product;
        }
        public override string ToString()
        {
            return $"Адрес - {Address}\nСтоимость - {Price}";
        }
    }
    class HomeDelivery : Delivery
    {
        public HomeDelivery(string address, Product product)
            : base(address, product) 
        { 
            Price = 500;

        }
    }

    class PickPointDelivery : Delivery
    {
        public PickPointDelivery(string address, Product product)
            : base(address, product) { Price = 200; }
    }

    class ShopDelivery : Delivery
    {
        public ShopDelivery(string address, Product product)
            : base(address, product) { Price = 0; }
    }

    class Order<TDelivery, TStruct> where TDelivery : Delivery
    {
        public TDelivery Delivery;
        public int NumberProducts { get; set; }//кол-во заказываемого товара
        public string Description;//название продукта_заказанное количество
        public OrderStatus OrderStatus;//статус заказа
        
        private double price;

        //если количество товара в наличии меньше, чем пользователь хочет заказать, то в заказ включается только то, что есть
        public Order(TDelivery delivery, int number)
        {
            Delivery = delivery;
            NumberProducts = number;
            Description = $"{delivery.Product.Name}_{NumberProducts}";
            //расчёт цены заказа в зависимости от наличия скидки
            price = (Delivery.Product.Discount != 0)
                ? Delivery.Product.Price * ((1 - (Delivery.Product.Discount / 100.00)) * NumberProducts) + 
                    Delivery.Price : (Delivery.Product.Price * NumberProducts) + Delivery.Price;
            //заказ поступил в работу
            OrderStatus = OrderStatus.Build;
        }

        public void DisplayAddress()
        {
            Console.WriteLine(Delivery.Address);
        }
        public void ChangeDeliveryStatus()
        {
            if (OrderStatus != OrderStatus.CanGet)
            {
                OrderStatus++;
            }
        }
        public override string ToString()
        {
            return $"Вы заказали {NumberProducts} {Delivery.Product.Name} на сумму {price:C2}. Статус заказа - {OrderStatus}.";
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
            int AmountAvailableProducts = For_Products.Next(10, 20);
            List<Product> Products = new List<Product>();
            for (int i = 0; i < AmountAvailableProducts; i++)
            {
                if (For_Products.Next(0, 2) == 0)
                {
                    Products.Add(new(Math.Round(For_Products.NextDouble(), 2) + For_Products.Next(100, 10000), For_Products.Next(1, 21), For_Products.Next(10, 30)));
                }
                else
                {
                    Products.Add(new(Math.Round(For_Products.NextDouble(), 2) + For_Products.Next(100, 10000), For_Products.Next(1, 21)));
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
        static void CreateOrder(ref Order<Delivery, int> order, List<Product> Products, 
            Delivery delivery, string enterProduct, string enterNum, string enterAddress)
        {
            Console.WriteLine("Итак, чтобы совершить, нужно ответить на несколько вопросов!");
            Console.Write("Введите название товара: ");

            do
            {
                DltSmthg._setCursorPosition(out int x, out int y);
                enterProduct = Console.ReadLine();
                enterProduct = enterProduct.ToUpper();
                if (Products.Find(x => x.Name == enterProduct) != null)
                {
                    EnterNumberProducts(out int count, Products, enterProduct, enterNum);


                    EnterNumber(out int choice, enterNum);
                    switch (choice)
                    {
                        case 1:
                            EnterAddress(ref enterAddress);
                            delivery = new ShopDelivery(enterAddress, Products.Find(x => x.Name == enterProduct));
                            break;

                        case 2:
                            EnterAddress(ref enterAddress);
                            delivery = new PickPointDelivery(enterAddress, Products.Find(x => x.Name == enterProduct));
                            break;

                        case 3:
                            EnterAddress(ref enterAddress);
                            delivery = new HomeDelivery(enterAddress, Products.Find(x => x.Name == enterProduct));
                            break;
                    }

                    order = new(delivery, count);
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
