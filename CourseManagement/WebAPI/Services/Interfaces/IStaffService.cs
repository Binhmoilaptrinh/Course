using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebAPI.DTOS.reponse;
using WebAPI.DTOS.request;

namespace WebAPI.Services.Interfaces
{
    public interface IStaffService
    {
        Task<StaffReponseDto> AddStaff(StaffRequestDto user);

        Task<StaffReponseDto> UpdateStaff(StaffReponseDto user);

        Task<List<StaffReponseDto>> GetStaffReponses();
    }
}
