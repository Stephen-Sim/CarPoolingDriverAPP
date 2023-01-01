using System;
using System.Collections.Generic;
using System.Text;

namespace CarPoolingDriverAPP.Models
{
    public class StatData
    {
        public string DayOfWeek { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public int TotalPassengers { get; set; }
        public decimal TotalEarned { get; set; }
    }
}
