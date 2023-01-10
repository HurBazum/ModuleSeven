using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleSeven
{
    internal class Customer
    {
        public string Name { get; set; }
        public double Count { get; set; }
        List<Order<Delivery, int>> MyIntOrders;
        List<Order<Delivery, string>> MyStringOrders;
        List<Bill> MyBills;
        public Customer(string name)
        {
            Name = name;
            MyIntOrders = new List<Order<Delivery, int>>();
            MyStringOrders = new List<Order<Delivery, string>>();
            MyBills = new List<Bill>();
        }
        public Customer(string name, double count) : this(name)
        {
            Count = count;
        }


    }
}
