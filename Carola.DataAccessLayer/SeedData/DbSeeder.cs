using Carola.DataAccessLayer.Context;
using Carola.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Carola.DataAccessLayer.SeedData
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarolaContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            // Migration'ları uygula (opsiyonel ama güvenli)
            await context.Database.MigrateAsync();

            // 1. Roller
            await SeedRolesAsync(roleManager);

            // 2. Admin kullanıcı
            await SeedAdminUserAsync(userManager);

            // 3. Brand'lar
            await SeedBrandsAsync(context);

            // 4. Category'ler
            await SeedCategoriesAsync(context);

            // 5. Location'lar
            await SeedLocationsAsync(context);

            // 6. WhyUs
            await SeedWhyUsAsync(context);

            // 7. Slider'lar
            await SeedSlidersAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            string[] roles = { "Admin", "Moderator" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            const string adminEmail = "admin@carola.com";
            const string adminPassword = "Admin123!";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "System",
                    Surname = "Admin",
                    EmailConfirmed = true,
                    ImageUrl = "/admin-assets/images/default-avatar.png"
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

        private static async Task SeedBrandsAsync(CarolaContext context)
        {
            if (await context.Brands.AnyAsync()) return;

            var brands = new List<Brand>
            {
                new() { BrandName = "BMW",           LogoUrl = "/assets/images/brands/bmw.png",           Status = true },
                new() { BrandName = "Mercedes-Benz", LogoUrl = "/assets/images/brands/mercedes.png",      Status = true },
                new() { BrandName = "Audi",          LogoUrl = "/assets/images/brands/audi.png",          Status = true },
                new() { BrandName = "Volkswagen",    LogoUrl = "/assets/images/brands/volkswagen.png",    Status = true },
                new() { BrandName = "Toyota",        LogoUrl = "/assets/images/brands/toyota.png",        Status = true },
                new() { BrandName = "Honda",         LogoUrl = "/assets/images/brands/honda.png",         Status = false },
                new() { BrandName = "Ford",          LogoUrl = "/assets/images/brands/ford.png",          Status = true },
                new() { BrandName = "Hyundai",       LogoUrl = "/assets/images/brands/hyundai.png",       Status = false },
                new() { BrandName = "Tesla",         LogoUrl = "/assets/images/brands/tesla.png",         Status = true },
                new() { BrandName = "Renault",       LogoUrl = "/assets/images/brands/renault.png",       Status = false }
            };

            await context.Brands.AddRangeAsync(brands);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCategoriesAsync(CarolaContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            var categories = new List<Category>
            {
                new() { CategoryName = "Sedan",     CategoryImage = "/assets/images/categories/sedan.png" },
                new() { CategoryName = "SUV",       CategoryImage = "/assets/images/categories/suv.png" },
                new() { CategoryName = "Truck",     CategoryImage = "/assets/images/categories/truck.png" },
                new() { CategoryName = "Sport",     CategoryImage = "/assets/images/categories/sport.png" },
                new() { CategoryName = "Hatchback", CategoryImage = "/assets/images/categories/hatchback.png" },
                new() { CategoryName = "Minivan",   CategoryImage = "/assets/images/categories/minivan.png" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedLocationsAsync(CarolaContext context)
        {
            if (await context.Locations.AnyAsync()) return;

            var locations = new List<Location>
            {
                new()
                {
                    LocationName = "İstanbul Havalimanı Şubesi",
                    AuthorizedPerson = "Ahmet Yılmaz",
                    City = "İstanbul",
                    Address = "Tayakadın, Terminal Cd., 34283 Arnavutköy/İstanbul",
                    ImageUrl = "/assets/images/locations/ist-airport.jpg"
                },
                new()
                {
                    LocationName = "Sabiha Gökçen Havalimanı Şubesi",
                    AuthorizedPerson = "Mehmet Demir",
                    City = "İstanbul",
                    Address = "Sanayi Mahallesi, Pendik/İstanbul",
                    ImageUrl = "/assets/images/locations/saw-airport.jpg"
                },
                new()
                {
                    LocationName = "Ankara Esenboğa Havalimanı Şubesi",
                    AuthorizedPerson = "Ali Kaya",
                    City = "Ankara",
                    Address = "Esenboğa Havalimanı, Çubuk/Ankara",
                    ImageUrl = "/assets/images/locations/ank-airport.jpg"
                },
                new()
                {
                    LocationName = "İzmir Adnan Menderes Şubesi",
                    AuthorizedPerson = "Ayşe Öz",
                    City = "İzmir",
                    Address = "Adnan Menderes Havalimanı, Gaziemir/İzmir",
                    ImageUrl = "/assets/images/locations/izm-airport.jpg"
                },
                new()
                {
                    LocationName = "Antalya Havalimanı Şubesi",
                    AuthorizedPerson = "Veli Şahin",
                    City = "Antalya",
                    Address = "Antalya Havalimanı, Muratpaşa/Antalya",
                    ImageUrl = "/assets/images/locations/ayt-airport.jpg"
                }
            };

            await context.Locations.AddRangeAsync(locations);
            await context.SaveChangesAsync();
        }

        private static async Task SeedWhyUsAsync(CarolaContext context)
        {
            if (await context.WhyUs.AnyAsync()) return;

            var whyUsItems = new List<WhyUs>
            {
                new()
                {
                    IconUrl = "/assets/images/icons/icon-1.png",
                    Title = "Farklı Araç Seçenekleri",
                    Description = "Ekonomik araçlardan lüks sedanlara, SUV'lardan sporlara kadar geniş filo yelpazesi."
                },
                new()
                {
                    IconUrl = "/assets/images/icons/icon-2.png",
                    Title = "7/24 Müşteri Desteği",
                    Description = "İhtiyaç duyduğunuz her an, deneyimli müşteri hizmetleri ekibimiz yanınızda."
                },
                new()
                {
                    IconUrl = "/assets/images/icons/icon-3.png",
                    Title = "Rekabetçi Fiyatlar",
                    Description = "Kaliteden ödün vermeden, pazarın en uygun kiralama fiyatlarını sunuyoruz."
                },
                new()
                {
                    IconUrl = "/assets/images/icons/icon-4.png",
                    Title = "Hızlı ve Kolay Rezervasyon",
                    Description = "Birkaç tıkla rezervasyonunuzu tamamlayın, aracınızı teslim alın."
                }
            };

            await context.WhyUs.AddRangeAsync(whyUsItems);
            await context.SaveChangesAsync();
        }

        private static async Task SeedSlidersAsync(CarolaContext context)
        {
            if (await context.Sliders.AnyAsync()) return;

            var sliders = new List<Slider>
            {
                new()
                {
                    Title = "Hayalinizdeki Aracı Kiralayın",
                    Description = "Lüks ve konforu uygun fiyatlarla sizlere sunuyoruz.",
                    BackgroundImage = "/assets/images/slider/slider-bg-1.jpg",
                    Image = "/assets/images/slider/slider-car-1.png",
                    Status = true
                },
                new()
                {
                    Title = "Premium Filo, Uygun Fiyat",
                    Description = "Geniş araç filomuzla her zevke uygun seçenekler sunuyoruz.",
                    BackgroundImage = "/assets/images/slider/slider-bg-2.jpg",
                    Image = "/assets/images/slider/slider-car-2.png",
                    Status = true
                },
                new()
                {
                    Title = "Güvenli ve Konforlu Sürüş",
                    Description = "Sigortalı ve bakımlı araçlarımızla güvenli yolculuklar.",
                    BackgroundImage = "/assets/images/slider/slider-bg-3.jpg",
                    Image = "/assets/images/slider/slider-car-3.png",
                    Status = true
                }
            };

            await context.Sliders.AddRangeAsync(sliders);
            await context.SaveChangesAsync();
        }
    }
}