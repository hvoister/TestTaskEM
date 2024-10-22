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
            if (File.Exists("test.txt"))
            {
                string filePath = "test.txt";
                string district;
                do
                {
                    Console.Write("Print a district: ");
                    district = Convert.ToString(Console.ReadLine());
                }
                while (String.IsNullOrEmpty(district));

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
                if (filtredOrders.Count > 0)
                {
                    foreach (var line in filtredOrders)
                    {
                        Console.WriteLine($"ID: {line.OrderID}\tWeight: {line.Weight}\tDistrict: {line.CityDistrict}\tTime: {line.DeliveryTime}\n");
                    }
                }
                else Console.WriteLine("There is no data to output.");
            }
            else Console.WriteLine("Add the data file to the project folder.");
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

            return orders.Where(o => o.CityDistrict == district && (filtredTime >= o.DeliveryTime && o.DeliveryTime >= firstDeliveryTime)).ToList();
        }
    }
}
