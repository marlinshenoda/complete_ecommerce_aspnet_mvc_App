using eTickets.Core.Entities;
using eTickets.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTickets.Data.Repositories
{
    public class ActorsRepository : EntityBaseRepository<Actor>, IActorsRepository
    {
        public ActorsRepository(ApplicationDbContext context) : base(context) { }

    }
}
