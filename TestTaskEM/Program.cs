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
            string logFilePath = "logs.txt";

            if (File.Exists("test.txt"))
            {
                string district;
                do
                {
                    Console.Write("Type a district: ");
                    district = Console.ReadLine().ToLower();
                    district = char.ToUpper(district[0]) + district.Substring(1);
                }
                while (String.IsNullOrEmpty(district));

                DateTime firstDeliveryTime;
                string inputTime;
                do
                {
                    Console.Write("Type the time of the first delivery (YYYY-MM-DD HH:MM:SS): ");
                    inputTime = Console.ReadLine();
                }
                while (!DateTime.TryParseExact(inputTime, "yyyy-MM-dd HH:mm:ss", null, DateTimeStyles.None, out firstDeliveryTime));

                var orders = GetOrders(filePath);
                Log($"Loaded {orders.Count} orders.", logFilePath);
                var filteredOrders = FilterOrders(orders, district, firstDeliveryTime);
                Log($"Found {filteredOrders.Count} orders for {district}.", logFilePath);

                Console.WriteLine();
                try
                {
                    if (filteredOrders.Count > 0)
                    { 
                        File.Delete($"test_{district}.txt");
                        Log("An old one result file is deleted.", logFilePath);
                        foreach (var line in filteredOrders)
                        {
                            Console.WriteLine($"ID: {line.OrderID}\tWeight: {line.Weight}\tDistrict: {line.CityDistrict}\tTime: {line.DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")}\n");
                            File.AppendAllText($"test_{district}.txt", $"ID: {line.OrderID}\tWeight: {line.Weight}\tDistrict: {line.CityDistrict}\tTime: {line.DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")}\n");
                        }
                        Log("Created the result file and log file in the project folder.", logFilePath);
                        Console.WriteLine("Created the result file and log file in the project folder.\n");
                    }
                    else Console.WriteLine("There is no data to output.");
                }
                catch (Exception) { 
                    Log("An error occurred when creating the result or log file.", logFilePath);
                }
            }
            else
            { 
                Console.WriteLine("Add the data file (test.txt) to the project folder.");
                Log($"Missing data file.", logFilePath);
            }
            Console.ReadKey();
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


        public static List<Orders> FilterOrders(List<Orders> orders, string district, DateTime firstDeliveryTime)
        {
            return orders.Where(o => o.CityDistrict == district && (firstDeliveryTime.AddMinutes(30) >= o.DeliveryTime && o.DeliveryTime >= firstDeliveryTime)).ToList();
        }

        public static void Log(string message, string logFilePath)
        {
            File.AppendAllText(logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}\n");
        }
    }
}
