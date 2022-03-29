#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Data;

    public class SaltimerDBContext : DbContext
    {
        public SaltimerDBContext (DbContextOptions<SaltimerDBContext> options)
            : base(options)
        {
        }

        public DbSet<Saltimer.Api.Data.User> User { get; set; }
    }
