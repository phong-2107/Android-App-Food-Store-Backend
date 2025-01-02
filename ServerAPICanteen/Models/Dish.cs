namespace ServerAPICanteen.Models
{
    public class Dish
    {
        public int IdDish { get; set; }
        public string DishName { get; set; }
        public string? PictureUrlArray { get; set; }
        public decimal? Amount { get; set; }
        public bool? DishStats { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }
        public int? IdCategory { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
