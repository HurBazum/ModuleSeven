﻿namespace ModuleSeven
{
    class Product
    {
        //имена продукции создаются рандомно
        Random ProductRandomName = new Random();
        string name;
        public string Name
        {
            get { return name; }
            private set//инициализируется в конструкторе, больше нигде не изменяется
            {
                value = "";
                int NameLength = ProductRandomName.Next(3, 10);
                for (int i = 0; i < NameLength; i++)
                {
                    value += (char)ProductRandomName.Next(65, 91);//добавление латинских букв в верхнем регистре в строку
                }
                name = value;
            }
        }
        public double Price { get; set; }
        public int Count { get; set; }
        public double Discount;

        public bool IsOrdered = default;
        public Product(double price, int count, double discount = default)//дефолтное значение скидки - 0
        {
            Name = Name;
            Price = price;
            Count = count;
            Discount = discount;
        }
        public void ChangeCount(int count)
        {
            //переменная IsOrdered меняет своё значение в конструкторах доставок на true
            //и при завершении формирования заказа в методе CreateOrder в классе Program обратно на false
            if (!IsOrdered)
            {
                Count += count;
            }
            else
            {
                Count -= count;
            }
        }
        public override string ToString()
        {
            return (Discount != default) 
                ? $"{Name,9} стоит {Price, 3}, скидка = {Discount}%, в наличии {Count}"
                    : $"{Name,9} стоит {Price, 3}, скидок нет, в наличии {Count}";
        }
    }
}
