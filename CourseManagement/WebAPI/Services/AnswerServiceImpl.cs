using AutoMapper;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services
{
    public class AnswerServiceImpl : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswerServiceImpl(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task<AnswerResponse> CreateAnswerAsync(AnswerRequestDto request)
        {
            var answer = _mapper.Map<Answer>(request);
            var answerCreated = await _answerRepository.AddAnswerAsync(answer);
            return _mapper.Map<AnswerResponse>(answerCreated);
        }
    }
}
