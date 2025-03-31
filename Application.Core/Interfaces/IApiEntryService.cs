using Application.Core.Dtos;
using Domain.Core.Entities;

namespace Application.Core.Interfaces;

public interface IApiEntryService
{
    Task<IEnumerable<ApiEntry>> GetAllApiEntries();
    Task UpsertApiEntries(List<ApiEntryDto> apiEntryDtos);
}