using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApiTest1.Models
{
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = new Random().Next(1, 100);
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [JsonIgnore]
        public int OrderResponseForeignKey { get; set; }
        [JsonIgnore]
        public OrderResponse OrderResponse { get; set; }
    }
}