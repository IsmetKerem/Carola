using AutoMapper;
using Carola.DtoLayer.BrandDtos;
using Carola.DtoLayer.CarDtos;
using Carola.DtoLayer.CategoryDtos;
using Carola.DtoLayer.CommentDtos;
using Carola.DtoLayer.ContactDtos;
using Carola.DtoLayer.CustomerDtos;
using Carola.DtoLayer.GalleryDtos;
using Carola.DtoLayer.LocationDtos;
using Carola.DtoLayer.ReservationDtos;
using Carola.DtoLayer.SliderDtos;
using Carola.DtoLayer.VideoDtos;
using Carola.DtoLayer.WhyUsDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // Brand
            CreateMap<Brand, ResultBrandDto>().ReverseMap();
            CreateMap<Brand, CreateBrandDto>().ReverseMap();
            CreateMap<Brand, UpdateBrandDto>().ReverseMap();
            CreateMap<Brand, GetByIdBrandDto>().ReverseMap();

            // Category
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetByIdCategoryDto>().ReverseMap();

            // Location
            CreateMap<Location, ResultLocationDto>().ReverseMap();
            CreateMap<Location, CreateLocationDto>().ReverseMap();
            CreateMap<Location, UpdateLocationDto>().ReverseMap();
            CreateMap<Location, GetByIdLocationDto>().ReverseMap();

            // Slider
            CreateMap<Slider, ResultSliderDto>().ReverseMap();
            CreateMap<Slider, CreateSliderDto>().ReverseMap();
            CreateMap<Slider, UpdateSliderDto>().ReverseMap();
            CreateMap<Slider, GetByIdSliderDto>().ReverseMap();

            // WhyUs
            CreateMap<WhyUs, ResultWhyUsDto>().ReverseMap();
            CreateMap<WhyUs, CreateWhyUsDto>().ReverseMap();
            CreateMap<WhyUs, UpdateWhyUsDto>().ReverseMap();
            CreateMap<WhyUs, GetByIdWhyUsDto>().ReverseMap();

            // Car — Brand ve Category navigation'lari ek mapping ile
            CreateMap<Car, ResultCarDto>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand != null ? s.Brand.BrandName : ""))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.CategoryName : ""))
                .ReverseMap();
            CreateMap<Car, CreateCarDto>().ReverseMap();
            CreateMap<Car, UpdateCarDto>().ReverseMap();
            CreateMap<Car, GetByIdCarDto>()
                .ForMember(d => d.BrandName, o => o.MapFrom(s => s.Brand != null ? s.Brand.BrandName : ""))
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.CategoryName : ""))
                .ReverseMap();

            // Reservation — navigation'lar
            CreateMap<Reservation, ResultReservationDto>()
                .ForMember(d => d.CarModel,           o => o.MapFrom(s => s.Car != null ? s.Car.Model : ""))
                .ForMember(d => d.BrandName,          o => o.MapFrom(s => s.Car != null && s.Car.Brand != null ? s.Car.Brand.BrandName : ""))
                .ForMember(d => d.CustomerFullName,   o => o.MapFrom(s => s.Customer != null ? s.Customer.FirstName + " " + s.Customer.LastName : ""))
                .ForMember(d => d.PickupLocationName, o => o.MapFrom(s => s.PickupLocation != null ? s.PickupLocation.LocationName : ""))
                .ForMember(d => d.ReturnLocationName, o => o.MapFrom(s => s.ReturnLocation != null ? s.ReturnLocation.LocationName : ""))
                .ForMember(d => d.CustomerFirstName,  o => o.MapFrom(s => s.Customer != null ? s.Customer.FirstName : ""))
                .ForMember(d => d.CustomerLastName,   o => o.MapFrom(s => s.Customer != null ? s.Customer.LastName : ""))
                .ForMember(d => d.CustomerEmail,      o => o.MapFrom(s => s.Customer != null ? s.Customer.Email : ""))
                .ForMember(d => d.CustomerPhone,      o => o.MapFrom(s => s.Customer != null ? s.Customer.Phone : ""))
                .ReverseMap();

            CreateMap<Reservation, CreateReservationDto>().ReverseMap();
            CreateMap<Reservation, UpdateReservationDto>().ReverseMap();
            CreateMap<Reservation, GetByIdReservationDto>().ReverseMap();
            // Comment
            CreateMap<Comment, ResultCommentDto>().ReverseMap();
            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
            CreateMap<Comment, GetByIdCommentDto>().ReverseMap();

            // Contact
            CreateMap<Contact, ResultContactDto>().ReverseMap();
            CreateMap<Contact, CreateContactDto>().ReverseMap();
            CreateMap<Contact, UpdateContactDto>().ReverseMap();
            CreateMap<Contact, GetByIdContactDto>().ReverseMap();

            // Gallery
            CreateMap<Gallery, ResultGalleryDto>().ReverseMap();
            CreateMap<Gallery, CreateGalleryDto>().ReverseMap();
            CreateMap<Gallery, UpdateGalleryDto>().ReverseMap();
            CreateMap<Gallery, GetByIdGalleryDto>().ReverseMap();

            // Video
            CreateMap<Video, ResultVideoDto>().ReverseMap();
            CreateMap<Video, CreateVideoDto>().ReverseMap();
            CreateMap<Video, UpdateVideoDto>().ReverseMap();
            CreateMap<Video, GetByIdVideoDto>().ReverseMap();

            // Customer
            CreateMap<Customer, ResultCustomerDto>().ReverseMap();
            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
            CreateMap<Customer, GetByIdCustomerDto>().ReverseMap();
            
            
        }
    }
}