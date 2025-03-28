using AutoMapper;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class QuestionServiceImpl : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerService _answerService;
        private readonly IMapper _mapper;

        public QuestionServiceImpl(IQuestionRepository questionRepository, IMapper mapper, IAnswerService answerService)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
            _answerService = answerService;
        }

        public async Task<QuestionResponse> CreateQuestionAsync(QuestionRequestDto request)
        {
            var question = _mapper.Map<Question>(request);
            var questionCreated = await _questionRepository.AddQuestionAsync(question);
            foreach (var answerDto in request.AnswersDto)
            {
                answerDto.QuestionId = questionCreated.Id;
                await _answerService.CreateAnswerAsync(answerDto);
            }
            return _mapper.Map<QuestionResponse>(questionCreated);
        }

        public async Task<QuestionResponse> UpdateQuestionAsync(QuestionUpdateDto request)
        {
            var question = await _questionRepository.GetQuestionByIdAsync(request.Id);
            _mapper.Map(request, question);
            var questionUpdated = await _questionRepository.UpdateQuestionAsync(question);
            foreach (var answerDto in request.AnswersDto)
            {
                if(answerDto.Id == null || answerDto.Id == 0)
                {
                    await _answerService.CreateAnswerAsync(new AnswerRequestDto
                    {
                        QuestionId = request.Id,
                        AnswerText = answerDto.AnswerText,
                        IsCorrect = answerDto.IsCorrect
                     });
                } else
                {
                    await _answerService.UpdateAnswerAsync(answerDto);
                }
            }
            return _mapper.Map<QuestionResponse>(questionUpdated);
        }
    }
}
