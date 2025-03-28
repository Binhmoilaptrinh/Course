using AutoMapper;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class LessonServiceImpl : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IQuestionService _questionService;
        private readonly IChapterRepository _chapterRepository;
        private readonly IUserService _userService;
        private readonly ECourseContext _context;

        public LessonServiceImpl(ILessonRepository lessonRepository, IMapper mapper,
            IChapterRepository chapterRepository, IUserService userService, IFileService fileService,
            IQuestionService questionService, ECourseContext context)
        {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
            _chapterRepository = chapterRepository;
            _userService = userService;
            _fileService = fileService;
            _questionService = questionService;
            _context = context;

        }

        public async Task<LessonVideoResponseAdmin> CreateLessonVideoAsync(LessonVideoRequestDto lessonDto)
        {
            var lesson = _mapper.Map<Lesson>(lessonDto);
            var user = await _userService.GetUserByIdAsync(1);//gia su ng 1 login

            lesson.Creator = user;
            lesson.CreatedAt = DateTime.Now;
            lesson.Type = "video";

            var videoBlob = await _fileService.UploadAsync(lessonDto.Video);
            lesson.VideoUrl = videoBlob.Blob.Uri.ToString();
            lesson = await _lessonRepository.CreateAsync(lesson);
            return _mapper.Map<LessonVideoResponseAdmin>(lesson);
        }

        public async Task<LessonQuizzResponseAdmin> CreateLessonQuizzAsync(LessonQuizzRequestDto lessonDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var lesson = _mapper.Map<Lesson>(lessonDto);
                    var user = await _userService.GetUserByIdAsync(1);
                    lesson.Creator = user;
                    lesson.CreatedAt = DateTime.Now;
                    lesson.Type = "quizz";
                    if (lessonDto.QuestionsDto != null && lessonDto.QuestionsDto.Count > 0)
                    {
                        lesson = await _lessonRepository.CreateAsync(lesson);
                        foreach (var questionDto in lessonDto.QuestionsDto)
                        {
                            questionDto.LessonId = lesson.Id;
                            await _questionService.CreateQuestionAsync(questionDto);
                        }
                    } else
                    {
                        throw new Exception("Cần cung cấp câu hỏi");
                    }
                   
                    await transaction.CommitAsync();
                    return _mapper.Map<LessonQuizzResponseAdmin>(lesson);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // Rollback nếu có lỗi
                    throw new Exception("Lỗi trong quá trình tạo Lesson", ex);
                }
            }
        }

        public async Task<IEnumerable<LessonResponseAdmin>> GetAllLessonAsync()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<LessonResponseAdmin>>(lessons);
        }

        public async Task<LessonDetailResponseAdmin> GetLessonByIdAsync(int id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);
            if (lesson == null)
            {
                throw new Exception("Not found");
            }
            return _mapper.Map<LessonDetailResponseAdmin>(lesson);
        }

        public async Task<LessonVideoResponseAdmin> UpdateLessonVideoAsync(int lessonId, LessonVideoUpdateDto request)
        {
            var existingLesson = await _lessonRepository.GetByIdAsync(lessonId);
            if(existingLesson == null)
            {
                throw new Exception($"Lesson with Id {lessonId} not found");
            }
            var user = await _userService.GetUserByIdAsync(request.UserId);//gia su ng 1 login => request can bien userId
            _mapper.Map(request, existingLesson);

            existingLesson.Updater = user;
            existingLesson.UpdatedAt = DateTime.Now;
            if(request.Video != null)
            {
                var videoBlob = await _fileService.UploadAsync(request.Video);
                existingLesson.VideoUrl = videoBlob.Blob.Uri.ToString();
            }

            var updateLesson = await _lessonRepository.UpdateAsync(existingLesson);
            return _mapper.Map<LessonVideoResponseAdmin>(updateLesson);
        }

        public async Task<LessonQuizzResponseAdmin> UpdateLessonQuizzAsync(int lessonId, LessonQuizzUpdateDto request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingLesson = await _lessonRepository.GetByIdAsync(lessonId);
                    _mapper.Map(request, existingLesson);
                    var user = await _userService.GetUserByIdAsync(request.UserId);
                    existingLesson.Updater = user;
                    existingLesson.UpdatedAt = DateTime.Now;
                    if (request.Type == "quizz" && request.QuestionsDto != null)
                    {
                        existingLesson = await _lessonRepository.UpdateAsync(existingLesson);
                        foreach (var questionDto in request.QuestionsDto)
                        {
                            if(questionDto.Id == 0 || questionDto.Id == null)
                            {
                                var createQues = new QuestionRequestDto
                                {
                                    LessonId = lessonId,
                                    QuestionText = questionDto.QuestionText,
                                    AnswersDto = questionDto.AnswersDto.Select(x => new AnswerRequestDto
                                    {
                                        AnswerText = x.AnswerText,
                                        IsCorrect = x.IsCorrect
                                    }).ToList()
                                };
                                await _questionService.CreateQuestionAsync(createQues);
                            } else
                            {
                                await _questionService.UpdateQuestionAsync(questionDto);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Cần cung cấp câu hỏi");
                    }

                    await transaction.CommitAsync();
                    return _mapper.Map<LessonQuizzResponseAdmin>(existingLesson);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // Rollback nếu có lỗi
                    throw new Exception("Lỗi trong quá trình tạo Lesson", ex);
                }
            }
        }
    }
}
