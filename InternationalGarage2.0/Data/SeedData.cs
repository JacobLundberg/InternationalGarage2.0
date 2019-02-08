using InternationalGarage2_0.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace InternationalGarage2_0.Controllers
{
    public class SeedData
    {
        const int NUMBER_SEEDED_MEMBERS = 20;
        const int CHANCE_MATCH_MEMBER_WITH_VEHICLE = 5; // The chance that a member will get a wehicle 1/X.
        const string VALID_LICENSE_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVXYZÅÄÖ";

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<InternationalGarage2_0Context>>();
            using (var context = new InternationalGarage2_0Context(options))
            {
                if (context.Member.Any())
                {
                    context.Member.RemoveRange(context.Member);
                    context.VehicleType.RemoveRange(context.VehicleType);
                    context.ParkedVehicle.RemoveRange(context.ParkedVehicle);
                }
                var members = new List<Member>();
                for (int i = 0; i < NUMBER_SEEDED_MEMBERS; i++)
                {
                    string name = Faker.Name.First() + " "+ Faker.Name.Last();
                    var person = new Member
                    {
                        Name = name
                    };
                    members.Add(person);
                }
                context.Member.AddRange(members);

                var textInfo = new CultureInfo("en-us", false).TextInfo;
                var vehicleTypes = new List<VehicleType>();
                vehicleTypes.Add(new VehicleType { Name = "Bus" });
                vehicleTypes.Add(new VehicleType { Name = "Car" });
                vehicleTypes.Add(new VehicleType { Name = "Motorcycle" });
                vehicleTypes.Add(new VehicleType { Name = "RV" });
                vehicleTypes.Add(new VehicleType { Name = "Truck" });
                context.VehicleType.AddRange(vehicleTypes);
                context.SaveChanges();

                var parkedVehicles = new List<ParkedVehicle>();
                string[] validColors = new string[] { "Black", "Blue", "Green", "Red", "Pink", "White" };
                string[] validModels = new string[] { "Suzuki", "Volvo", "SAAB", "BMW", "Ford" };
                try
                {
                foreach (var member in members)
                {
                    foreach (var vehicleType in vehicleTypes)
                    {
                        if (Faker.RandomNumber.Next(CHANCE_MATCH_MEMBER_WITH_VEHICLE) == 0)
                        {
                            var pv = new ParkedVehicle
                            {
                                Type= vehicleType.Id,
                                VehicleTypeId = vehicleType.Id,
                                MemberId = member.Id,
                                LicenseNumber = VALID_LICENSE_LETTERS.Substring(Faker.RandomNumber.Next(VALID_LICENSE_LETTERS.Length), 1)
                                + VALID_LICENSE_LETTERS.Substring(Faker.RandomNumber.Next(VALID_LICENSE_LETTERS.Length), 1)
                                + VALID_LICENSE_LETTERS.Substring(Faker.RandomNumber.Next(VALID_LICENSE_LETTERS.Length), 1)
                                + Faker.RandomNumber.Next(1000).ToString("000"),
                                Color = validColors[Faker.RandomNumber.Next(validColors.Length)],
                                Model = validModels[Faker.RandomNumber.Next(validModels.Length)],
                                NumberOfWheels = (Faker.RandomNumber.Next(3) + 1) * 2,
                                TimeStampCheckIn = DateTime.Now.AddMinutes(-Faker.RandomNumber.Next(6000)),
                                TimeStampCheckOut = null
                            };
                            parkedVehicles.Add(pv);
                        }
                    }
                }
                context.ParkedVehicle.AddRange(parkedVehicles);
                context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}









/*



public class Seed
    {
        private readonly InternationalGarage2_0Context _context;

        public Seed(InternationalGarage2_0Context context)
        {
            _context = context;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                SeedMember();  // If in development & db is empty -> seed some
                SeedVehicleType();
                SeedParkedVehice();  // If in development & db is empty -> seed some
            }
        }

        /// <summary>
        /// Add data seed into members table only if it has no data.
        /// </summary>
        private void SeedMember()
        {
            if (_context.Member.Count() != 0) return;

            var people = new List<Member>();
            people.Add(new Member { Name = "Cooper Botsford" });
            people.Add(new Member { Name = "Kaelyn Christiansen" });
            people.Add(new Member { Name = "Felipe Pacocha" });
            people.Add(new Member { Name = "Sherwood Will" });
            people.Add(new Member { Name = "Carlos Reynolds" });
            _context.Member.AddRange(people);
            _context.SaveChanges();
        }

        /// <summary>
        /// Seed some mockup vehicleTypes into database.
        /// </summary>
        private void SeedVehicleType()
        {
            if (_context.VehicleType.Count() != 0) return;
            var vehicles = new List<VehicleType>();
            vehicles.Add(new VehicleType { Name = "Bus" });
            vehicles.Add(new VehicleType { Name = "Car" });
            vehicles.Add(new VehicleType { Name = "Motorcycle" });
            vehicles.Add(new VehicleType { Name = "RV" });
            vehicles.Add(new VehicleType { Name = "Truck" });
            _context.VehicleType.AddRange(vehicles);
            _context.SaveChanges();
        }

        /// <summary>
        /// Seed some mockup vehicles into database.
        /// </summary>
        protected void SeedParkedVehice()
        {
            if (_context.ParkedVehicle.Count() != 0) return;
            var member = _context.Member.FirstOrDefault();
            var vehicle = _context.VehicleType.FirstOrDefault();
            _context.ParkedVehicle.Add(new ParkedVehicle()
            {
                Type = -1,//VehicleType2.OldCar,
                LicenseNumber = "BUZ987",
                Color = "Red",
                Model = "MAN",
                NumberOfWheels = 6,
                TimeStampCheckIn = new DateTime(2019, 01, 24, 22, 55, 21),
                TimeStampCheckOut = null,
                MemberId = member.Id,
                VehicleTypeId = vehicle.Id,
            });
            _context.ParkedVehicle.Add(new ParkedVehicle()
            {
                Type = -1,//VehicleType2.OldBus,
                LicenseNumber = "BUZ666",
                Color = "Red",
                Model = "MAN",
                NumberOfWheels = 6,
                TimeStampCheckIn = new DateTime(2019, 01, 24, 22, 55, 21),
                TimeStampCheckOut = null,
                MemberId = member.Id,
                VehicleTypeId = vehicle.Id,
            });
            _context.ParkedVehicle.Add(new ParkedVehicle()
            {
                Type = -1,//VehicleType2.OldBus,
                LicenseNumber = "XXX666",
                Color = "Grey",
                Model = "MAN",
                NumberOfWheels = 6,
                TimeStampCheckIn = new DateTime(2019, 02, 14, 22, 55, 21),
                TimeStampCheckOut = null,
                MemberId = member.Id,
                VehicleTypeId = vehicle.Id,
            }); _context.SaveChanges();
        }

    }
}

    */
