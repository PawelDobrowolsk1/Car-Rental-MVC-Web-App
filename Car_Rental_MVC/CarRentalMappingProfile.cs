using AutoMapper;
using Car_Rental_MVC.Entities;
using Car_Rental_MVC.Models;

namespace Car_Rental_MVC
{
    public class CarRentalMappingProfile : Profile
    {
        public CarRentalMappingProfile()
        {
            CreateMap<Car, CarModelDto>();
            CreateMap<CarModelDto, Car>();
            CreateMap<User, UserModelDto>()
                .ForMember(d => d.Role, u => u.MapFrom(r => r.Role.Name));
        }
    }
}
