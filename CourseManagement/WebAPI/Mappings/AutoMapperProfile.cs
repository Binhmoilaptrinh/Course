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

            CreateMap<UserRequestDto, User>();
            CreateMap<User, UserReponseDto>(); // Fixed naming inconsistency
            CreateMap<UserReponseDto, User>();
            CreateMap<UserRoleRequest, UserRole>();
            CreateMap<UserRole, UserRoleResponseDto>();
            CreateMap<DiscountRequestDto, Discount>();
            CreateMap<Discount, DiscountResponseDto>();
            CreateMap<CategoryRequestDto, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<Course, CourseClientDTO>();
          
         

            CreateMap<Course, CourseDetailResponseDto>()
                .ForMember(dest => dest.ChapterResponse, opt => opt.MapFrom(src => src.Chapters));


            CreateMap<CourseRequestDto, Course>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Skip null values when mapping

            CreateMap<ChapterRequestDto, Chapter>();
            CreateMap<Chapter, ChapterResponse>();
            CreateMap<Chapter, ChapterDetailResponse>()
               .ForMember(dest => dest.LessonResponse, opt => opt.MapFrom(src => src.Lessons));

            CreateMap<LessonQuizzRequestDto, Lesson>();

            CreateMap<LessonVideoRequestDto, Lesson>();

            CreateMap<LessonQuizzUpdateDto, Lesson>();

            CreateMap<LessonVideoUpdateDto, Lesson>();

            CreateMap<Lesson, LessonQuizzResponseAdmin>()
                .ForMember(dest => dest.QuestionResponse, opt => opt.MapFrom(src => src.Questions));

            CreateMap<Lesson, LessonVideoResponseAdmin>();
            CreateMap<Lesson, LessonDetailResponseAdmin>()
                .ForMember(dest => dest.QuestionResponse, opt => opt.MapFrom(src => src.Questions));

            CreateMap<QuestionRequestDto, Question>();
            CreateMap<QuestionUpdateDto, Question>();

            CreateMap<Question, QuestionResponse>()
                .ForMember(dest => dest.AnswerResponse, opt => opt.MapFrom(src => src.Answers));


            CreateMap<AnswerRequestDto, Answer>();
            CreateMap<AnswerUpdateDto, Answer>();
            CreateMap<Answer, AnswerResponse>();

            CreateMap<Lesson, LessonResponseAdmin>();
              
        }
    }
}
