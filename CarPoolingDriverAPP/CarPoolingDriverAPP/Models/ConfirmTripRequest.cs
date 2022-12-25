using System;
using System.Collections.Generic;
using System.Text;

namespace CarPoolingDriverAPP.Models
{
    public class ConfirmTripRequest
    {
        public int TripId { get; set; }
        public List<int> RequestsId { get; set; }
    }
}
