using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ServerAPICanteen.Models
{
    public class User : IdentityUser
    {
        public string? Fullname { get; set; }
        public byte? Sex { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? AccountStats { get; set; }
        public string? GoogleAuth { get; set; }
        public string? FacebookAuth { get; set; }
        public DateTime? CreDate { get; set; }
        public bool? Active { get; set; }
        public string? Initials { get; set; }

        // Navigation properties
        public ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
    }
}
