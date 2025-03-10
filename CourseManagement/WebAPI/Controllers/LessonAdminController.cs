﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost("add/video")]
        public async Task<ActionResult<LessonVideoResponseAdmin>> CreateLessonVideo([FromForm] LessonVideoRequestDto request)
        {
            var lesson = await _lessonService.CreateLessonVideoAsync(request);
            return CreatedAtAction(nameof(GetLessonDetail), new { id = lesson.Id }, lesson);
        }

        [HttpPost("add/quizz")]
        public async Task<ActionResult<LessonQuizzResponseAdmin>> CreateLessonQuizz([FromBody] LessonQuizzRequestDto request)
        {
            var lesson = await _lessonService.CreateLessonQuizzAsync(request);
            return CreatedAtAction(nameof(GetLessonDetail), new { id = lesson.Id }, lesson);
        }

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<LessonResponseAdmin>> GetAllLesson()
        {
            var lessons = await _lessonService.GetAllLessonAsync();
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonResponseAdmin>> GetLessonDetail(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            return Ok(lesson);
        }
    }
}
