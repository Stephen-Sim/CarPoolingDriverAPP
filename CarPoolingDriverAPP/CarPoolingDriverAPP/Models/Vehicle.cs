using System;
using System.Collections.Generic;
using System.Text;

namespace CarPoolingDriverAPP.Models
{
    public class Vehicle
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string PlatNo { get; set; }
        public string Color { get; set; }
        public int Capacity { get; set; }
    }
}
