using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
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
        private readonly ECourseContext _courseContext;

        public CourseServiceImpl(ICourseRepository courseRepository, 
            IMapper mapper, IFileService fileService, IUserService userService,
             ICategoryService categoryService, ECourseContext courseContext)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _fileService = fileService;
            _userService = userService;
            _categoryService = categoryService;
            _courseContext = courseContext;
        }

        public async Task<IQueryable<CourseAdminResponseDto>> GetAllCourse()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.AsQueryable().ProjectTo<CourseAdminResponseDto>(_mapper.ConfigurationProvider);
        }

        public async Task<CourseAdminResponseDto> CreateCourseAsync(CourseRequestDto request)
        {
            try
            {
                if(! await _categoryService.IsExistByIdAsync(request.CategoryId))
                {
                    throw new Exception($"Category with Id {request.CategoryId} not found");
                }
                var user = await _userService.GetUserByIdAsync(request.CreateBy);
                var thumbnailBlob = await _fileService.UploadAsync(request.Thumbnail);
                var previewVideoBlob = await _fileService.UploadAsync(request.PreviewVideo);

                string thumbnail = thumbnailBlob.Blob.Uri.ToString();
                string previewVideo = previewVideoBlob.Blob.Uri.ToString();

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
                    var thumbnailBlob = await _fileService.UploadAsync(request.Thumbnail);
                    existingCourse.Thumbnail = thumbnailBlob.Blob.Uri.ToString();
                }
                if (request.PreviewVideo != null)
                {
                    var previewVideoBlob = await _fileService.UploadAsync(request.PreviewVideo);
                    existingCourse.PreviewVideo = previewVideoBlob.Blob.Uri.ToString();
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

        public async Task<CourseDetailResponseDto> GetCourseByIdAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                throw new Exception($"Course with Id {id} not found");
            }
            return _mapper.Map<CourseDetailResponseDto>(course);
        }

        
        public async Task<bool> IsExistCourseByIdAsync(int id)
        {
            return await _courseRepository.IsExistByIdAsync(id);
        }

        public async Task<CourseDetailAdmin> GetCourseDetailAdmin(int id)
        {
            var courseDetail = await _courseContext.Courses.Include(x=>x.Category).Select(c => new CourseDetailAdmin
            {
                CourseId = c.Id,
                Title = c.Title,
                Thumbnail = c.Thumbnail,
                CategoryName = c.Category.Name,
                Price = c.Price,
                Enrollments = _courseContext.Enrollments.Where(x=>x.CourseId == id).Count(),
                LessonsCount = _courseContext.Lessons.Where(l => l.Chapter.CourseId == id).Count(),
                Status = c.Status
            }).FirstOrDefaultAsync(x => x.CourseId == id);
            return courseDetail;
        }
    }
}
