using Microsoft.EntityFrameworkCore;
using MiniTwit.Models.DataModels;

namespace MiniTwit.Data
{
    public class MiniTwitContext : DbContext
    {
        public MiniTwitContext(DbContextOptions<MiniTwitContext> options)
            : base(options) { }

        public DbSet<Twit> Twits => Set<Twit>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Follower> Followers => Set<Follower>();
    }
}
