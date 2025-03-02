using AutoMapper;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class CourseServiceImpl : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CourseServiceImpl(ICourseRepository courseRepository, IMapper mapper, IFileService fileService, IUserService userService)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _fileService = fileService;
            _userService = userService;
        }

        public async Task<IEnumerable<CourseAdminResponseDto>> GetAllCourseAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CourseAdminResponseDto>>(courses);
        }

        public async Task<CourseAdminResponseDto> CreateCourseAsync(CourseCreateDto request)
        {
            try
            {
                if (request.Thumbnail?.Length > 512 * 1024 * 1024)
                {
                    throw new InvalidDataException("File size should not exceed 512 MB");
                }
                string[] allowedFileExtentions = [".jpg", ".jpeg", ".png", ".mp4"];
                string thumbnail = await _fileService.SaveFileAsync(request.Thumbnail, allowedFileExtentions);
                string previewVideo = await _fileService.SaveFileAsync(request.PreviewVideo, allowedFileExtentions);
                var user = await _userService.GetUserByIdAsync(1);

                var course = _mapper.Map<Course>(request);
                course.Thumbnail = thumbnail;
                course.PreviewVideo = previewVideo;
                course.Creator = user;
                var courseCreated = await _courseRepository.CreateAsync(course);
                return _mapper.Map<CourseAdminResponseDto>(courseCreated);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
