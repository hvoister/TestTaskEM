using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskEM
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }


        public List<Orders> GetOrders(string filePath)
        {
            var orders = new List<Orders>();

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(';');
                if (parts.Length != 4) continue;

                if (DateTime.TryParse(parts[3], out DateTime deliveryTime) && double.TryParse(parts[1], out double weight) && int.TryParse(parts[0], out int orderID))
                {
                    orders.Add(new Orders
                    {
                        OrderID = orderID,
                        Weight = weight,
                        CityDistrict = parts[2],
                        DeliveryTime = deliveryTime
                    });

                }
            }
            return orders;
        }


        public List<Orders> OrdersFiltration(List<Orders> orders, string district, DateTime firstDeliveryTime)
        {
            DateTime filtredTime = firstDeliveryTime.AddMinutes(30);

            return orders.Where(o => o.CityDistrict == district && o.DeliveryTime <= filtredTime).ToList();

        }
    }
}
