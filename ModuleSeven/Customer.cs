using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
                else
                {
                    number = "номер не указан";
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
        //оплата
        public void PayTheBill(object? order_index)
        {
            if(order_index is int)
            {
                var order = MyIntOrders.Find(x => x.Id == (int)order_index);
                MyBills.Add(new(order.ToString()));
                MyBills[MyBills.Count - 1].ChangeBillInfo(this, ref order);
            }
            if(order_index is string)
            {
                var order = MyStringOrders.Find(x => x.Id == (string)order_index);
                MyBills.Add(new(order.ToString()));
                MyBills[MyBills.Count - 1].ChangeBillInfo(this, ref order);
            }
        }

        public override string ToString()
        {
            return $"Здравствуйте, {Name}, {Number}. У вас на счету {Count:C2}.";
        }
        public void ShowBills()
        {
            foreach(var bill in MyBills)
            {
                Console.WriteLine(bill);
            }
        }
    }
}
