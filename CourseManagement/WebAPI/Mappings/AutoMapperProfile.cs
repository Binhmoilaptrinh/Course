using AutoMapper;
using WebAPI.DTOS.response;
using WebAPI.DTOS.request;
using WebAPI.Models;
using WebAPI.DTOS.reponse;

namespace WebAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<StaffRequestDto, User>();
            CreateMap<User, StaffReponseDto>(); // Fixed naming inconsistency
            CreateMap<StaffReponseDto, User>();
            CreateMap<UserRoleRequest, UserRole>();
            CreateMap<UserRole, UserRoleResponseDto>();
            CreateMap<DiscountRequestDto, Discount>();
            CreateMap<Discount, DiscountResponseDto>();
            CreateMap<CategoryRequestDto, Category>();
            CreateMap<Category, CategoryResponse>();
            
            CreateMap<Course, CourseAdminResponseDto>()
                .ForMember(dest => dest.categoryResponse, opt => opt.MapFrom(src => src.Category));
            
            CreateMap<CourseRequestDto, Course>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Skip null values when mapping

            CreateMap<ChapterRequestDto, Chapter>();
            CreateMap<Chapter, ChapterResponse>();
        }
    }
}
