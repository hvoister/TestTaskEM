using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskEM
{
    public class Program
    {
        static void Main(string[] args)
        {
            string filePath = "test.txt";
            Console.Write("Print a district: ");
            string district = Convert.ToString(Console.ReadLine());
            DateTime firstDeliveryTime;
            string inputTime;
            do
            {
                Console.Write("Print a delivery time (YYYY-MM-DD HH:MM:SS): ");
                inputTime = Console.ReadLine();
            }
            while (!DateTime.TryParseExact(inputTime, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out firstDeliveryTime));


            var orders = GetOrders(filePath);
            var filtredOrders = OrdersFiltration(orders, district, firstDeliveryTime);

            Console.WriteLine();
            foreach (var line in orders)
            {
                Console.WriteLine($"ID: {line.OrderID}\tWeight: {line.Weight}\tDistrict: {line.CityDistrict}\tTime: {line.DeliveryTime}\n");
            }
        }


        public static List<Orders> GetOrders(string filePath)
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


        public static List<Orders> OrdersFiltration(List<Orders> orders, string district, DateTime firstDeliveryTime)
        {
            DateTime filtredTime = firstDeliveryTime.AddMinutes(30);

            return orders.Where(o => o.CityDistrict == district && o.DeliveryTime <= filtredTime).ToList();

        }
    }
}
