using System;
using System.Collections.Generic;
using System.Text;

namespace CarPoolingDriverAPP.Models
{
    public class RequestRequest
    {
        public int? Id { get; set; }
        public int? TripRequestId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string RequestNumber { get; set; } = string.Empty;
        public decimal? Charges { get; set; }
        public decimal? FromLatitude { get; set; }
        public decimal? FromLongitude { get; set; }
        public string FromAddress { get; set; } = string.Empty;
        public decimal? ToLatitude { get; set; }
        public decimal? ToLongitude { get; set; }
        public string ToAddress { get; set; } = string.Empty;
        public int? NumberOfPassengers { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string DiplayFromAddress { get; set; } = string.Empty;
        public string DisplayToAddress { get; set; } = string.Empty;
    }
}
