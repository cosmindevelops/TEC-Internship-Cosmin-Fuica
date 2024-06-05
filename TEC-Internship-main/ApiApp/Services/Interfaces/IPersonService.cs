using ApiApp.Common.Dto;

namespace ApiApp.Services.Interfaces;

public interface IPersonService
{
    Task<IEnumerable<PersonDto>> GetAllPersonsAsync();

    Task<PersonDto> CreatePersonAsync(CreateUpdatePersonDto personDto);

    Task<bool> UpdatePersonAsync(int personId, CreateUpdatePersonDto personDto);

    Task<bool> DeletePersonAsync(int personId);

    Task<PersonDto> GetPersonAsync(int personId);
}