using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OtzarSffarrim.Models;

namespace OtzarSffarrim.Data
{
    public class OtzarSffarrimContext : DbContext
    {
        public OtzarSffarrimContext (DbContextOptions<OtzarSffarrimContext> options)
            : base(options)
        {
        }

        public DbSet<OtzarSffarrim.Models.Library> Library { get; set; } = default!;
        public DbSet<OtzarSffarrim.Models.Shelf> Shelf { get; set; } = default!;
        public DbSet<OtzarSffarrim.Models.Book> Book { get; set; } = default!;
    }
}
