using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Concrete;

public class CarolaContext:DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=45.87.120.36,1433;Database=CarolaRentDb;User Id=sa;Password=StrongPass123!;TrustServerCertificate=True;\");\n    }");
    }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}