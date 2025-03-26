using AutoMapper;
using Azure.Core;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class ChapterServiceImpl : IChapterService
    {
        private readonly IChapterRepository _chapterRepository;
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ChapterServiceImpl(IChapterRepository chapterRepository, ICourseService courseService, IMapper mapper, IUserService userService)
        {
            _chapterRepository = chapterRepository;
            _courseService = courseService;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ChapterResponse> CreateChapterAsync(ChapterRequestDto request)
        {
            if (!await _courseService.IsExistCourseByIdAsync(request.CourseId))
            {
                throw new Exception($"Course with id {request.CourseId} not exist");
            }
            var user = await _userService.GetUserByIdAsync(1);
            var chapter = _mapper.Map<Chapter>(request);
            chapter.Creator = user;
            var chapterCreated = await _chapterRepository.CreateAsync(chapter);
            return _mapper.Map<ChapterResponse>(chapterCreated);
        }

        public async Task DeleteChapterById(int id)
        {
            if (await _chapterRepository.GetByIdAsync(id) == null)
            {
                throw new Exception($"Chapter with Id {id} not found");
            }
            await _chapterRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ChapterResponse>> GetAllChapterAsync()
        {
            var chapters = await _chapterRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ChapterResponse>>(chapters);
        }

        public async Task<ChapterResponse> GetChapterByIdAsync(int id)
        {
            var chapter = await _chapterRepository.GetByIdAsync(id);
            if (chapter == null)
            {
                throw new Exception($"Chapter with Id {id} not found");
            }
            return _mapper.Map<ChapterResponse>(chapter);
        }

        public async Task<ChapterResponse> UpdateChapterAsync(int id, ChapterRequestDto request)
        {
            var existingChapter = await _chapterRepository.GetByIdAsync(id);
            if (existingChapter == null)
            {
                throw new Exception($"Chapter with Id {id} not found");
            }

            if (!await _courseService.IsExistCourseByIdAsync(request.CourseId))
            {
                throw new Exception($"Course with Id {id} not found");
            }

            _mapper.Map(request, existingChapter);
            var user = await _userService.GetUserByIdAsync(1);
            existingChapter.Updater = user;
            existingChapter.UpdatedAt = DateTime.Now;
            var updateChapter = await _chapterRepository.UpdateAsync(existingChapter);
            return _mapper.Map<ChapterResponse>(updateChapter);
        }
    }
}