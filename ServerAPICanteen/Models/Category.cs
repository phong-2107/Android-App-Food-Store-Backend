namespace ServerAPICanteen.Models
{
    public class Category
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }

        // Navigation properties
        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
