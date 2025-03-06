using AutoMapper;
<<<<<<< HEAD
using WebAPI.DTOS.reponse;
using WebAPI.DTOS.request;
using WebAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
=======
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
>>>>>>> 87b1073cfd82b875b0b5b5b2d6f1d83de6a1a9f6

namespace WebAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
<<<<<<< HEAD
            CreateMap<StaffRequestDto, User>();
            CreateMap<User, StaffReponseDto>();
            CreateMap<StaffReponseDto, User>();
            CreateMap<UserRoleRequest, UserRole>();
            CreateMap<UserRole, UserRoleResponseDto>();
            CreateMap<DiscountRequestDto, Discount>();
            CreateMap<Discount, DiscountResponseDto>();

=======
            CreateMap<CategoryRequestDto, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<Course, CourseAdminResponseDto>()
                .ForMember(dest => dest.categoryResponse, otp => otp.MapFrom(src => src.Category));
            CreateMap<CourseRequestDto, Course>()
                .ForAllMembers(opt => opt.Condition((src, data, srcMember) => srcMember != null));//bỏ qua giá trị null khi mapping

            CreateMap<ChapterRequestDto, Chapter>();
            CreateMap<Chapter, ChapterResponse >();
>>>>>>> 87b1073cfd82b875b0b5b5b2d6f1d83de6a1a9f6
        }
    }
}
