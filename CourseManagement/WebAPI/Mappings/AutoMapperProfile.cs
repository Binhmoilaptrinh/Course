using AutoMapper;
using WebAPI.DTOS.response;
using WebAPI.DTOS.request;
using WebAPI.Models;

namespace WebAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<Course, CourseAdminResponseDto>();

            CreateMap<StaffRequestDto, User>();
            CreateMap<User, StaffReponseDto>(); // Fixed naming inconsistency
            CreateMap<StaffReponseDto, User>();
            CreateMap<UserRoleRequest, UserRole>();
            CreateMap<UserRole, UserRoleResponseDto>();
            CreateMap<DiscountRequestDto, Discount>();
            CreateMap<Discount, DiscountResponseDto>();
            CreateMap<CategoryRequestDto, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<Course, CourseClientDTO>();
          
           
         
            CreateMap<Course, CourseAdminResponseDto>()
                .ForMember(dest => dest.categoryResponse, opt => opt.MapFrom(src => src.Category));
            

            CreateMap<CourseRequestDto, Course>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Skip null values when mapping

            CreateMap<ChapterRequestDto, Chapter>();
            CreateMap<Chapter, ChapterResponse>();


            CreateMap<LessonQuizzRequestDto, Lesson>();

            CreateMap<LessonVideoRequestDto, Lesson>();

            CreateMap<Lesson, LessonQuizzResponseAdmin>()
                .ForMember(dest => dest.QuestionResponse, opt => opt.MapFrom(src => src.Questions));

            CreateMap<Lesson, LessonVideoResponseAdmin>();

            CreateMap<QuestionRequestDto, Question>();
            CreateMap<Question, QuestionResponse>()
                .ForMember(dest => dest.AnswerResponse, opt => opt.MapFrom(src => src.Answers));


            CreateMap<AnswerRequestDto, Answer>();
            CreateMap<Answer, AnswerResponse>();

            CreateMap<Lesson, LessonResponseAdmin>()
               .ForMember(dest => dest.QuestionResponse, opt => opt.MapFrom(src => src.Questions));
        }
    }
}
