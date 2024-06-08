using ApiApp.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Interfaces;

public interface IPersonService
{
    Task<IEnumerable<WebApp.Models.PersonDto>> GetAllPersonsAsync();

    Task<bool> CreatePersonAsync(CreateUpdatePersonDto personDto);

    Task<bool> UpdatePersonAsync(int personId, CreateUpdatePersonDto personDto);

    Task<bool> DeletePersonAsync(int personId);
}