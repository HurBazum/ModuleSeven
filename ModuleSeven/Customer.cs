using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ModuleSeven
{
    internal class Customer
    {
        string number;
        public string Name { get; set; }
        public string Number 
        {
            get
            {
                return number;
            }
            set
            {
                if (Regex.IsMatch(value, @"\d{10}"))
                {
                    number = value;
                }
            }
        }
        public double Count { get; set; }
        public List<Order<Delivery, int>> MyIntOrders;
        public List<Order<Delivery, string>> MyStringOrders;
        public List<Bill> MyBills;
        public Customer(string name)
        {
            Name = name;
            MyIntOrders = new List<Order<Delivery, int>>();
            MyStringOrders = new List<Order<Delivery, string>>();
            MyBills = new List<Bill>();
        }
        public Customer(string name, double count, string phone_number) : this(name)
        {
            Number = phone_number;
            Count = count;
        }
        public void PayTheBill(object order_index)
        {
            if(order_index is int)
            {
                var order = MyIntOrders.Find(x => x.Id == (int)order_index);
                MyBills.Add(new(order.ToString()));
                order.ChangeDeliveryStatus();
                MyBills[MyBills.Count - 1].SetPayed(this, ref order);
            }
            if(order_index is string)
            {
                var order = MyStringOrders.Find(x => x.Id == (string)order_index);
                MyBills.Add(new(order.ToString()));
                order.ChangeDeliveryStatus();
                MyBills[MyBills.Count - 1].SetPayed(this, ref order);
            }
        }

        public override string ToString()
        {
            return $"Здравствуйте, {Name}. У вас на счету {Count}.\n{MyBills[0].ToString()}";
        }
    }
}
