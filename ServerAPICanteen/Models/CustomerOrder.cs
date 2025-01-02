namespace ServerAPICanteen.Models
{
    public class CustomerOrder
    {
        public int IdOrder { get; set; }
        public string? Id { get; set; }
        public int? IdCanteen { get; set; }
        public string? QRCodeValue { get; set; }
        public DateTimeOffset? CreDate { get; set; }
        public decimal? Amount { get; set; }
        public double? VATValue { get; set; }
        public decimal? FinalAmount { get; set; }
        public byte? PaidMethod { get; set; }
        public bool? PaidStats { get; set; }
        public string? SpecialNote { get; set; }
        public bool? IsOrderReceived { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalAmount { get; set; }

        // Navigation properties
        public User? User { get; set; } 
        public Canteen? Canteen { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
