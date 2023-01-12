using Car_Rental_MVC.Entities;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_MVC
{
    public class CarRentalSeeder
    {
        private readonly CarRentalManagerContext _dbContext;

        public CarRentalSeeder(CarRentalManagerContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            var pendingMigrations = _dbContext.Database.GetPendingMigrations();
            if (pendingMigrations != null && pendingMigrations.Any())
            {
                _dbContext.Database.Migrate();
            }

            if (_dbContext.Database.CanConnect())
            {
                

                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Cars.Any())
                {
                    var cars = GetCars();
                    _dbContext.Cars.AddRange(cars);
                    _dbContext.SaveChanges();
                }

                if(!_dbContext.Users.Any())
                {
                    var admin = GetAdmin();
                    _dbContext.Users.Add(admin);
                    _dbContext.SaveChanges();
                }
            }
        }

        private User GetAdmin()
        {
            var admin = new User()
            {
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.com",
                PasswordHash = "AQAAAAEAACcQAAAAEBRD56y8qwwcO418F+27FZzHwdH+SbDfGE6/baUq4afdTqalI/JzR5dKpvchm+mgVg==",
                RoleId = 3
            };
            return admin;
        }

        private IEnumerable<Car> GetCars()
        {
            var cars = new List<Car>()
            {
                new Car()
                { 
                    Category = "Sedan",
                    Make = "Alfa Romeo",
                    Model = "159",
                    Engine = 2.4M,
                    Horsepower = 200,
                    Year = 2006,
                    Seats = 5,
                    Doors = 4,
                    Fuel = "ON",
                    Transmisson = "Manual"

                },
                new Car()
                {
                    Category = "Hatchback",
                    Make = "Fiat",
                    Model = "Bravo II",
                    Engine = 1.4M,
                    Horsepower = 150,
                    Year = 2008,
                    Seats = 5,
                    Doors = 5,
                    Fuel = "PB",
                    Transmisson = "Manual"

                },
                new Car()
                {
                    Category = "Coupe",
                    Make = "BMW",
                    Model = "Z3",
                    Engine = 4.4M,
                    Horsepower = 200,
                    Year = 1999,
                    Seats = 2,
                    Doors = 2,
                    Fuel = "PB",
                    Transmisson = "Manual"

                },
                new Car()
                {
                    Category = "Hatchback",
                    Make = "Volvo",
                    Model = "V40 II",
                    Engine = 2.5M,
                    Horsepower = 200,
                    Year = 2012,
                    Seats = 5,
                    Doors = 5,
                    Fuel = "PB",
                    Transmisson = "Automatic"

                },
                new Car()
                {
                    Category = "Sedan",
                    Make = "BMW",
                    Model = "E90",
                    Engine = 3.0M,
                    Horsepower = 200,
                    Year = 1999,
                    Seats = 5,
                    Doors = 5,
                    Fuel = "PB",
                    Transmisson = "Manual"

                },
                new Car()
                {
                    Category = "Sedan",
                    Make = "Lexus",
                    Model = "ES",
                    Engine = 2.0M,
                    Horsepower = 200,
                    Year = 2019,
                    Seats = 5,
                    Doors = 5,
                    Fuel = "PB",
                    Transmisson = "Manual"

                },
                new Car()
                {
                    Category = "Hatchback",
                    Make = "Peugeot",
                    Model = "307",
                    Engine = 1.6M,
                    Horsepower = 110,
                    Year = 2006,
                    Seats = 5,
                    Doors = 5,
                    Fuel = "PB",
                    Transmisson = "Manual"

                }

            };

            return cars;
        }

        public IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }
    }
}
