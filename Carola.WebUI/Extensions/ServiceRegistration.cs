using Carola.BusinessLayer.Abstract;
using Carola.BusinessLayer.Concrete;
using Carola.BusinessLayer.Mapping;
using Carola.DataAccessLayer.Repositories.BrandRepository;
using Carola.DataAccessLayer.Repositories.CarRepository;
using Carola.DataAccessLayer.Repositories.CategoryRepository;
using Carola.DataAccessLayer.Repositories.CommentRepository;
using Carola.DataAccessLayer.Repositories.ContactRepository;
using Carola.DataAccessLayer.Repositories.CustomerRepository;
using Carola.DataAccessLayer.Repositories.GalleryRepository;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.DataAccessLayer.Repositories.LocationRepository;
using Carola.DataAccessLayer.Repositories.ReservationRepository;
using Carola.DataAccessLayer.Repositories.SliderRepository;
using Carola.DataAccessLayer.Repositories.VideoRepository;
using Carola.DataAccessLayer.Repositories.WhyUsRepository;

namespace Carola.WebUI.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCarolaServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(GeneralMapping).Assembly);

            // Generic DAL
            services.AddScoped(typeof(IGenericDal<>), typeof(GenericRepository<>));

            // Brand
            services.AddScoped<IBrandDal, EfBrandDal>();
            services.AddScoped<IBrandService, BrandManager>();

            // Category
            services.AddScoped<ICategoryDal, EfCategoryDal>();
            services.AddScoped<ICategoryService, CategoryManager>();

            // Location
            services.AddScoped<ILocationDal, EfLocationDal>();
            services.AddScoped<ILocationService, LocationManager>();

            // Slider
            services.AddScoped<ISliderDal, EfSliderDal>();
            services.AddScoped<ISliderService, SliderManager>();

            // WhyUs
            services.AddScoped<IWhyUsDal, EfWhyUsDal>();
            services.AddScoped<IWhyUsService, WhyUsManager>();

            // Car
            services.AddScoped<ICarDal, EfCarDal>();
            services.AddScoped<ICarService, CarManager>();

            // Reservation
            services.AddScoped<IReservationDal, EfReservationDal>();
            services.AddScoped<IReservationService, ReservationManager>();

            // Comment
            services.AddScoped<ICommentDal, EfCommentDal>();
            services.AddScoped<ICommentService, CommentManager>();

            // Contact
            services.AddScoped<IContactDal, EfContactDal>();
            services.AddScoped<IContactService, ContactManager>();

            // Gallery
            services.AddScoped<IGalleryDal, EfGalleryDal>();
            services.AddScoped<IGalleryService, GalleryManager>();

            // Video
            services.AddScoped<IVideoDal, EfVideoDal>();
            services.AddScoped<IVideoService, VideoManager>();

            // Customer
            services.AddScoped<ICustomerDal, EfCustomerDal>();
            services.AddScoped<ICustomerService, CustomerManager>();

            return services;
        }
    }
}