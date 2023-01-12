namespace ModuleSeven
{
    class Product
    {
        //имена продукции создаются рандомно
        Random ProductRandomName = new Random();
        string name;//название товара
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
        //уменьшает количество товара, при оплате в классе Bill
        //также есть возможность увеличивать это кол-во
        public void ChangeCount(int count)
        {
            if (IsOrdered == false)
            {
                Count += count;
            }
            else
            {
                Count -= count;
                IsOrdered = false;
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
