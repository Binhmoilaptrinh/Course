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
            CreateMap<Course, CourseAdminResponseDto>();
            CreateMap<CourseRequestDto, Course>()
                .ForAllMembers(opt => opt.Condition((src, data, srcMember) => srcMember != null));//bỏ qua giá trị null khi mapping

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
