using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTickets.Core.Entities
{
#nullable disable

    public class Order
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
