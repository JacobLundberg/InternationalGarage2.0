using Microsoft.EntityFrameworkCore;

namespace InternationalGarage2_0.Models
{
    public class InternationalGarage2_0Context : DbContext
    {
        public InternationalGarage2_0Context (DbContextOptions<InternationalGarage2_0Context> options)
            : base(options)
        {
        }

        public DbSet<InternationalGarage2_0.Models.ParkedVehicle> ParkedVehicle { get; set; }

        public DbSet<InternationalGarage2_0.Models.Member> Member { get; set; }

        public DbSet<InternationalGarage2_0.Models.VehicleType> VehicleType { get; set; }
    }
}
