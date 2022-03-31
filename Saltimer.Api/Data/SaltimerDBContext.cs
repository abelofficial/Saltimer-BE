#nullable disable
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Data;

    public class SaltimerDBContext : DbContext
    {
        public SaltimerDBContext (DbContextOptions<SaltimerDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Saltimer.Api.Data.MobTimer> MobTimer { get; set; }

        public DbSet<Saltimer.Api.Data.UserMobSession> UserMobSession { get; set; }


    }
