using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChapterController : Controller
    {
        private readonly IChapterService _chapterService;

        public ChapterController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        [HttpPost]
        public async Task<ActionResult<ChapterResponse>> CreateChapter(ChapterRequestDto request)
        {
            var chapter = await _chapterService.CreateChapterAsync(request);
            return Ok(chapter);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ChapterResponse>> UpdateChapter(int id, ChapterRequestDto request)
        {
            var chapter = await _chapterService.UpdateChapterAsync(id, request);
            return Ok(chapter);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChapterResponse>>> GetAllChapter()
        {
            var chapters = await _chapterService.GetAllChapterAsync();
            return Ok(chapters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChapterResponse>> GetChapterDetailChapter(int id)
        {
            var chapter = await _chapterService.GetChapterByIdAsync(id);
            return Ok(chapter);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChapterById(int id)
        {
            await _chapterService.DeleteChapterById(id);
            return Ok("Delete successfully");
        }
    }
}
