using Confetti.Domain.Entities;

namespace Confetti.Infrastructure.EntityFrameworkCore.IntegrationTests.Domain
{
    public class Post : Entity<int>
    {
        public string Name { get; set; }

        public Blog Blog { get; set; }
    }
}