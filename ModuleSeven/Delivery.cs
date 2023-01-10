namespace ModuleSeven
{
    abstract class Delivery
    {
        public string Address { get; set; }
        public string deliveryType;
        public string DeliveryType { get { return deliveryType; } }
        public double Price;
        public DateTime deliveryDate;
        public int DaysForDeliver;
        public Product Product { get; set; }

        public virtual DateTime DeliveryDate { get; set; }
        public Delivery()
        {

        }
        public Delivery(string address, ref Product product)
        {
            deliveryType = "";
            Address = address;
            Product = product;
        }
        public override string ToString()
        {
            return $"{DeliveryType}\n{Address}\n{DeliveryDate}\n{Product.ToString()}";
        }
    }
}
