using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTickets.Core.Entities
{
    public class ShoppingCardItem
    {
        public int Id { get; set; }

        public Movie Movie { get; set; }
        public int Amount { get; set; }


        public string ShoppingCartId { get; set; }
    }
}
