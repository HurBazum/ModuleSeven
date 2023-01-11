using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleSeven
{
    internal class Bill
    {
        bool isPayed;
        public string BillStatus { get; set; }

        public Bill(string order)
        {
            isPayed = false;
        }

        public void SetPayed(Customer c, ref Order<Delivery, int> order)
        {
            if (c.Count >= order.Price && isPayed == false)
            {
                isPayed = true;
                c.Count -= order.Price;
                order.ChangeDeliveryStatus();
                BillStatus = "Оплачено\n" + order.ToString();
                c.MyBills.Add(this);
            }
            else
            {

                BillStatus = "Ожидает оплаты\n" + order;
                c.MyBills.Add(this);
            }
        }
        public void SetPayed(Customer c, ref Order<Delivery, string> order)
        {
            if (c.Count >= order.Price && isPayed == false)
            {
                isPayed = true;
                c.Count -= order.Price;
                order.ChangeDeliveryStatus();
                BillStatus = "Оплачено\n" + order.ToString();
                c.MyBills.Add(this);
            }
            else
            {

                BillStatus = "Ожидает оплаты\n" + order;
                c.MyBills.Add(this);
            }
        }
        public override string ToString()
        {
            return BillStatus;
        }
    }
}
