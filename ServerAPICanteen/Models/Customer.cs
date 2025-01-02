namespace ServerAPICanteen.Models
{
    public class Customer
    {
        public string IdCustomer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Fullname { get; set; }
        public byte? Sex { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? AccountStats { get; set; }
        public string? GoogleAuth { get; set; }
        public string? FacebookAuth { get; set; }
        public DateTime? CreDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool? Active { get; set; }

        // Navigation properties
        public ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
    }
}
