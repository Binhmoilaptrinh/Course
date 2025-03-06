using AutoMapper;
using Azure.Core;
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
        private readonly ICategoryService _categoryService;
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

        public async Task<CourseAdminResponseDto> CreateCourseAsync(CourseRequestDto request)
        {
            try
            {
                if(! await _categoryService.IsExistByIdAsync(request.CategoryId))
                {
                    throw new Exception($"Category with Id {request.CategoryId} not found");
                }
                var user = await _userService.GetUserByIdAsync(1);
                string thumbnail = await ValidateFile(request.Thumbnail);
                string previewVideo = await ValidateFile(request.PreviewVideo);
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

        public async Task<CourseAdminResponseDto> UpdateCourseAsync(int id, CourseRequestDto request)
        {
            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse == null)
            {
                throw new Exception($"Course with Id {id} not found");
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(1);
                _mapper.Map(request, existingCourse);
                existingCourse.Updater = user;
                if (request.Thumbnail != null)
                {
                    string thumbnail = await ValidateFile(request.Thumbnail);
                    existingCourse.Thumbnail = thumbnail;
                }
                if (request.PreviewVideo != null)
                {
                    string previewVideo = await ValidateFile(request.PreviewVideo);
                    existingCourse.PreviewVideo = previewVideo;
                }
                existingCourse.UpdatedAt = DateTime.Now;
                var updateCourse = await _courseRepository.UpdateAsync(existingCourse);
                return _mapper.Map<CourseAdminResponseDto>(updateCourse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            
            if (! await _courseRepository.IsExistByIdAsync(id))
            {
                throw new Exception($"Course with Id {id} not found");
            }
            await _courseRepository.DeleteAsync(id);
        }

        public async Task<CourseAdminResponseDto> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                throw new Exception($"Course with Id {id} not found");
            }
            return _mapper.Map<CourseAdminResponseDto>(course);
        }

        public async Task<string> ValidateFile(IFormFile file)
        {
            if (file?.Length > 512 * 1024 * 1024)
            {
                throw new InvalidDataException("File size should not exceed 512 MB");
            }
            string[] allowedFileExtentions = [".jpg", ".jpeg", ".png", ".mp4"];
            return await _fileService.SaveFileAsync(file, allowedFileExtentions);
        }

        public async Task<bool> IsExistCourseByIdAsync(int id)
        {
            return await _courseRepository.IsExistByIdAsync(id);
        }
    }
}
