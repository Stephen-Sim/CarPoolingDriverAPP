using System;
using System.Collections.Generic;
using System.Text;

namespace CarPoolingDriverAPP.Models
{
    public class DriverInfo
    {
        public string DriverName { get; set; }
        public byte[] DriverProfile { get; set; }
        public string Rating { get; set; }
        public string TotalEarned { get; set; }
    }
}
