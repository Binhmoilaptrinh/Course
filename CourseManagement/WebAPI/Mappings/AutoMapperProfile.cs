using AutoMapper;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;

namespace WebAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryRequestDto, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<Course, CourseAdminResponseDto>()
                .ForMember(dest => dest.categoryResponse, otp => otp.MapFrom(src => src.Category));
            CreateMap<CourseRequestDto, Course>()
                .ForAllMembers(opt => opt.Condition((src, data, srcMember) => srcMember != null));//bỏ qua giá trị null khi mapping

            CreateMap<ChapterRequestDto, Chapter>();
            CreateMap<Chapter, ChapterResponse >();
        }
    }
}
