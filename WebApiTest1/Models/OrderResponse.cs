using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiTest1.Models
{
    public class OrderResponse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = new Random().Next(1, 100);
        public string Dimension { get; set; }
        public string Status { get; set; } = Statuses.Status1.ToString("g");
        public enum Statuses { Status1, Status2, Status3 }
        public Location Pickup { get; set; }
        public Location DropOff { get; set; }
    }
}