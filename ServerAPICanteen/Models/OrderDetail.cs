namespace ServerAPICanteen.Models
{
    public class OrderDetail
    {
        public int IdOrder { get; set; }
        public int IdDish { get; set; }
        public int? Quantity { get; set; }
        public decimal? TotalAmount { get; set; }

        // Navigation properties
        public CustomerOrder? CustomerOrder { get; set; }
        public Dish? Dish { get; set; }
    }
}
