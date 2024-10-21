using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskEM
{
    public class Orders
    {
        public int OrderID { get; set; }
        public double Weight { get; set; }
        public string CityDistrict { get; set; }
        public DateTime DeliveryTime { get; set; }      // yyyy-MM-dd HH:mm:ss.

    }
}
