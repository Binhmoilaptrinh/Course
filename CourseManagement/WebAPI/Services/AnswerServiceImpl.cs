using AutoMapper;
using WebAPI.DTOS.request;
using WebAPI.DTOS.response;
using WebAPI.Models;
using WebAPI.Repositories;
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

        public async Task<AnswerResponse> UpdateAnswerAsync(AnswerUpdateDto request)
        {
            var answer = await _answerRepository.GetAnswerByIdAsync(request.Id);
            _mapper.Map(request, answer);
            var answerUpdated = await _answerRepository.UpdateAnswerAsync(answer);
            return _mapper.Map<AnswerResponse>(answerUpdated);
        }
    }
}
