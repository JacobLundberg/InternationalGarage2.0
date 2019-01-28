using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InternationalGarage2_0.Models;

namespace InternationalGarage2._0.Models
{
    public class InternationalGarage2_0Context : DbContext
    {
        public InternationalGarage2_0Context (DbContextOptions<InternationalGarage2_0Context> options)
            : base(options)
        {
        }

        public DbSet<InternationalGarage2_0.Models.ParkedVehicle> ParkedVehicle { get; set; }
    }
}
