using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Dtos;
using Application.Core.Interfaces;
using Domain.Core.Entities;
using Infrastructure.Persistence.Repositories;

namespace Application.Core.Services;

public class ApiEntryService : IApiEntryService
{
    private readonly ApiEntryRepository _apiEntryRepository;

    public ApiEntryService(ApiEntryRepository apiEntryRepository)
    {
        _apiEntryRepository = apiEntryRepository;
    }

    public async Task<IEnumerable<ApiEntry>> GetAllApiEntries()
    {
        return await _apiEntryRepository.GetAllAsync();
    }

    public async Task UpsertApiEntries(List<ApiEntryDto> apiEntryDtos)
    {
        await _apiEntryRepository.ClearTable();

        var validApiEntries = apiEntryDtos.Where(dto => !string.IsNullOrEmpty(dto.Link)).ToList(); 

        var apiEntries = new List<ApiEntry>();
        foreach (var dto in validApiEntries)
        {
            apiEntries.Add(new ApiEntry
            {
                API = dto.API,
                Description = dto.Description,
                Auth = dto.Auth,
                HTTPS = dto.HTTPS,
                Cors = dto.Cors,
                Link = dto.Link,
                Category = dto.Category
            });
        }

        await _apiEntryRepository.AddRangeAsync(apiEntries);
    }
}