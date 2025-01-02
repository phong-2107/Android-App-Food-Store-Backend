namespace ServerAPICanteen.Models
{
    public class Canteen
    {
        public int IdCanteen { get; set; }
        public string CanteenName { get; set; }
        public string? Address { get; set; }
        public string? BuildingName { get; set; }
        public string? LogoUrl { get; set; }
        public byte? ActiveStats { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreDate { get; set; }
        public bool? Active { get; set; }

        // Navigation properties
        public ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
    }
}
