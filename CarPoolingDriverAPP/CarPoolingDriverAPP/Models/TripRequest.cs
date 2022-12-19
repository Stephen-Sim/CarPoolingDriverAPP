using System;
using System.Collections.Generic;
using System.Text;

namespace CarPoolingDriverAPP.Models
{
    public class TripRequest
    {
        public int? Id { get; set; }
        public string TripNumber { get; set; } = string.Empty;
        public decimal? FromLatitude { get; set; }
        public decimal? FromLongitude { get; set; }
        public string FromAddress { get; set; } = string.Empty;
        public decimal? ToLatitude { get; set; }
        public decimal? ToLongitude { get; set; }
        public string ToAddress { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? VehicleId { get; set; }
        public string DisplayNumOfPerson { get; set; } = string.Empty;
        public string DiplayFromAddress { get; set; } = string.Empty;
        public string DisplayToAddress { get; set; } = string.Empty;
        public string DisplayTime { get; set; } = string.Empty;
    }
}
