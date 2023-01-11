using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ModuleSeven
{
    internal class Bill
    {
        public bool IsPayed { get; set; }
        public string BillStatus { get; set; }

        public Bill(string order)
        {
            BillStatus = "Ожидает оплаты\n" + order.ToString();
            IsPayed = false;
        }
        //public void ChangeBillInfo<T>(Customer c, ref Order<Delivery, T> order) where T : struct
        //не получается...
        public void ChangeBillInfo(Customer c, ref Order<Delivery, int> order)
        {
            if (c.Count >= order.Price && IsPayed == false)
            {
                IsPayed = true;
                c.Count -= order.Price;
                order.ChangeDeliveryStatus();
                BillStatus = "Оплачено\n" + order.ToString();
                c.MyBills.Add(this);
            }
            else if (BillStatus.Contains("Оплачено\n"))
            {
                order.ChangeDeliveryStatus();
                BillStatus = "Оплачено\n" + order.ToString();
            }
            else if(c.Count < order.Price)
            {
                BillStatus = $"Недостаточно средств, внесите на счёт ещё {order.Price - c.Count:C2}";
            }
        }
        public void ChangeBillInfo(Customer c, ref Order<Delivery, string> order)
        {
            if (c.Count >= order.Price && IsPayed == false)
            {
                IsPayed = true;
                c.Count -= order.Price;
                order.ChangeDeliveryStatus();
                BillStatus = "Оплачено\n" + order.ToString();
            }
            if (BillStatus.Contains("Оплачено\n"))
            {
                order.ChangeDeliveryStatus();
                BillStatus = "Оплачено\n" + order.ToString();
            }
            else if (c.Count < order.Price)
            {
                BillStatus = $"Недостаточно средств, внесите на счёт ещё {order.Price - c.Count:C2}";
            }
        }
        public override string ToString()
        {
            return BillStatus;
        }
    }
}
