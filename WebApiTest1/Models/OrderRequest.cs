namespace WebApiTest1.Models
{
    public class OrderRequest
    {
        public string Dimension { get; set; }
        public Location Pickup { get; set; }
        public Location DropOff { get; set; }
    }
}