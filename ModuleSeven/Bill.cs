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
        //изменяет информацию о статусе заказа в чеке
        //1 - заказ ещё не оплачен(но есть возможность это сделать)
        //2 - заказ уже оплачен(но не получен)
        //3 - недостаточно средств
        public void ChangeBillInfo<T>(Customer c, ref Order<Delivery, T> order)
        {
            if (c.Count >= order.Price && IsPayed == false)
            {
                IsPayed = true;//заказ оплачен
                c.Count -= order.Price;
                order.ChangeDeliveryStatus();
                //уменьшение кол-ва товаров в магазине происходит, только после оплаты
                order.Delivery.Product.IsOrdered = true; 
                order.Delivery.Product.ChangeCount(order.NumberProducts);
                //изменяет дату доставки, т.к. заказ мог быть сформирован, раньше, чем оплачен
                order.Delivery.DeliveryDate = DateTime.Now.AddDays(order.Delivery.DeliversDays);
                                                                                                
                BillStatus = "Оплачено\n" + order.ToString();
            }
            else if (BillStatus.Contains("Оплачено\n"))
            {
                order.ChangeDeliveryStatus();
                BillStatus = "Оплачено\n" + order.ToString();
            }
            else if(c.Count < order.Price)
            {
                BillStatus = $"Недостаточно средств, внесите на счёт ещё {order.Price - c.Count:C2}" + order.ToString();
            }
        }
        public override string ToString()
        {
            return BillStatus;
        }
    }
}
