using System.Collections.Generic;
using Confetti.Domain.Entities;

namespace Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Domain
{
    public class Blog : Entity<int>
    {
        public string Name { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}