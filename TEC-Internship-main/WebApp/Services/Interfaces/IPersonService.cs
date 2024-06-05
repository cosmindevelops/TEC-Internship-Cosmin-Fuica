using ApiApp.Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Interfaces;

public interface IPersonService
{
    Task<List<PersonDto>> GetPersonsAsync();
}