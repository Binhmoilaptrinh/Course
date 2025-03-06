using AutoMapper;
using WebAPI.DTOS.reponse;
using WebAPI.DTOS.request;
using WebAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<StaffRequestDto, User>();
            CreateMap<User, StaffReponseDto>();
            CreateMap<StaffReponseDto, User>();
            CreateMap<UserRoleRequest, UserRole>();
            CreateMap<UserRole, UserRoleResponseDto>();
            CreateMap<DiscountRequestDto, Discount>();
            CreateMap<Discount, DiscountResponseDto>();

        }
    }
}
