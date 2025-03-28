using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using WebApp.Models;

namespace WebApp.Pages.Admin.Course
{
    public class LessonModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public LessonModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public LessonDetailResponseAdmin LessonDetail { get; set; } = new LessonDetailResponseAdmin();
        public async Task<IActionResult> OnGetAsync(int? lessonId, int chapterId)
        {
            LessonDetail.ChapterId = chapterId;

            if (lessonId != null)
            {
                var apiUrl = $"https://api.2handshop.id.vn/api/Lesson/{lessonId}";
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    LessonDetail = JsonSerializer.Deserialize<LessonDetailResponseAdmin>(jsonResponse,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddVideoLessonAsync(int LessonId, string ChapterId,
            string LessonName, string Desc, IFormFile VideoFile, double Passing, double Duration, int UserId)
        {
            using var requestContent = new MultipartFormDataContent();

            if (VideoFile != null)
            {
                byte[] data;
                using (var br = new BinaryReader(VideoFile.OpenReadStream()))
                {
                    data = br.ReadBytes((int)VideoFile.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Video", VideoFile.FileName);
            }


            requestContent.Add(new StringContent(LessonName ?? ""), "Name");
            requestContent.Add(new StringContent("active"), "Status");
            requestContent.Add(new StringContent(UserId.ToString()), "UserId");
            requestContent.Add(new StringContent(Passing.ToString()), "Passing");
            requestContent.Add(new StringContent(Duration.ToString()), "Duration");
            requestContent.Add(new StringContent(Desc ?? ""), "Content");
            requestContent.Add(new StringContent(ChapterId.ToString()), "ChapterId");

            var response = new HttpResponseMessage();

            if (LessonId == 0)
            {
                response = await _httpClient.PostAsync("https://api.2handshop.id.vn/api/Lesson/add/video", requestContent);
            }
            else
            {
                response = await _httpClient.PutAsync("https://api.2handshop.id.vn/api/Lesson/update/video/" + LessonId, requestContent);
            }


            var jsonResponse = await response.Content.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.IsSuccessStatusCode)
            {
                if (LessonId == 0)
                {
                    LessonDetail = JsonSerializer.Deserialize<LessonDetailResponseAdmin>(jsonResponse, option);
                    TempData["SuccessMessage"] = "Thêm khóa học thành công!";
                }
                else
                {
                    LessonDetail = JsonSerializer.Deserialize<LessonDetailResponseAdmin>(jsonResponse, option);
                    TempData["SuccessMessage"] = "Cập nhật khóa học thành công!";
                }

                return Redirect("?lessonId=" + LessonDetail.Id + "&chapterId=" + LessonDetail.ChapterId);
            }
            else
            {
                if (LessonId == 0)
                {
                    TempData["ErrorMessage"] = "Thêm khóa học thất bại!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật khóa học thất bại!";
                }

                return Redirect("?chapterId=" + ChapterId);
            }
        }

        public async Task<IActionResult> OnPostAddQuizLessonAsync(int LessonId, int ChapterId,
            string LessonName, string Desc, List<QuestionUpdateDto> QuestionResponse, int UserId, float Passing, float Duration)
        {
            LessonQuizzUpdateDto LessonQuizzUpdateDto = new LessonQuizzUpdateDto
            {
                Id = LessonId,
                UserId = UserId,
                Name = LessonName,
                ChapterId = ChapterId,
                Type = "quizz",
                Status = "active",
                Content = Desc,
                Passing = Passing,
                Duration = Duration,
                QuestionsDto = QuestionResponse
            };
            using var jsonContent = new StringContent(JsonSerializer.Serialize(LessonQuizzUpdateDto), Encoding.UTF8, "application/json");
            var response = new HttpResponseMessage();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            if (LessonId == 0)
            {
                response = await _httpClient.PostAsync("https://api.2handshop.id.vn/api/Lesson/add/quizz", jsonContent);
            }
            else
            {
                response = await _httpClient.PutAsync("https://api.2handshop.id.vn/api/Lesson/update/quizz/" + LessonId, jsonContent);
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                if (LessonId == 0)
                {
                    LessonDetail = JsonSerializer.Deserialize<LessonDetailResponseAdmin>(jsonResponse, option);
                    TempData["SuccessMessage"] = "Thêm bài test thành công!";
                }
                else
                {
                    LessonDetail = JsonSerializer.Deserialize<LessonDetailResponseAdmin>(jsonResponse, option);
                    TempData["SuccessMessage"] = "Cập nhật bài test thành công!";

                }

                return Redirect("?lessonId=" + LessonDetail.Id + "&chapterId=" + LessonDetail.ChapterId);
            }
            else
            {
                if (LessonId == 0)
                {
                    TempData["ErrorMessage"] = "Thêm bài test thất bại!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Cập nhật bài test thất bại!";
                }

                return Redirect("?chapterId=" + ChapterId);
            }
        }

        //public IActionResult OnGetDeleteAnswer(int AnswerId, int LessonId)
        //{
        //    var response = new HttpResponseMessage();

        //    if (LessonId == 0)
        //    {
        //        response = await _httpClient.DeleteAsync("https://api.2handshop.id.vn/api/Lesson");
        //    }

        //    if (isDelete)
        //    {
        //        TempData["ToastMessage"] = "Xóa câu trả lời thành công";
        //        TempData["ToastType"] = "success";
        //    }
        //    else
        //    {
        //        TempData["ToastMessage"] = "Answer not found.";
        //        TempData["ToastType"] = "fail";
        //    }
        //    return RedirectToAction("", new { lessonId = lessonId });
        //}

        ////delete Quizz Question | delete Quizz Question
        //public IActionResult OnGetDeleteQuestion(int QuestionId, int LessonId)
        //{

        //    bool isDelete = _questionService.DeleteQuestionById(questionId);
        //    if (isDelete)
        //    {
        //        TempData["ToastMessage"] = "Xóa câu hỏi thành công.";
        //        TempData["ToastType"] = "success";
        //    }
        //    else
        //    {
        //        TempData["ToastMessage"] = "Question not found.";
        //        TempData["ToastType"] = "fail";
        //    }
        //    return RedirectToAction("", new { lessonId = lessonId });
        //}
    }
}
