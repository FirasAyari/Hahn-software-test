
using Application.Core.Dtos;
using Application.Core.Interfaces;
using Newtonsoft.Json;

namespace Infrastructure.Hangfire.Jobs;

public class ApiDataUpsertJob
{
    private readonly IApiEntryService _apiEntryService;
    private readonly HttpClient _httpClient;

    public ApiDataUpsertJob(IApiEntryService apiEntryService, HttpClient httpClient)
    {
        _apiEntryService = apiEntryService;
        _httpClient = httpClient;
    }

    public async Task Execute()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://dogapi.dog/api/v2/breeds");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var apiData = JsonConvert.DeserializeObject<DogApiResponse>(json);

            var apiEntryDtos = MapApiDataToDtos(apiData.Data);

            await _apiEntryService.UpsertApiEntries(apiEntryDtos);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ApiDataUpsertJob: {ex.Message}");
        }
    }

    private List<ApiEntryDto> MapApiDataToDtos(List<DogApiData> dogApiData)
    {
        var apiEntryDtos = new List<ApiEntryDto>();
        foreach (var dog in dogApiData)
        {
            apiEntryDtos.Add(new ApiEntryDto
            {
                API = dog.Attributes.Name,
                Description = dog.Attributes.Description,
                Auth = dog.Attributes.Hypoallergenic.ToString(),
                HTTPS = true,
                Cors = "unknown",
                Link = dog.Links?.Self ?? "N/A", 
                Category = dog.Relationships?.Group?.Data?.Type
            });
        }
        return apiEntryDtos;
    }

    public class DogApiResponse
    {
        public List<DogApiData> Data { get; set; }
        public DogApiMeta Meta { get; set; }
        public DogApiLinks Links { get; set; }
    }

    public class DogApiData
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DogApiAttributes Attributes { get; set; }
        public DogApiRelationships Relationships { get; set; }
        public DogApiLinks Links { get; set; }
    }

    public class DogApiAttributes
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DogApiLife Life { get; set; }
        public DogApiWeight Male_weight { get; set; }
        public DogApiWeight Female_weight { get; set; }
        public bool Hypoallergenic { get; set; }
    }

    public class DogApiLife
    {
        public int Max { get; set; }
        public int Min { get; set; }
    }

    public class DogApiWeight
    {
        public int Max { get; set; }
        public int Min { get; set; }
    }

    public class DogApiRelationships
    {
        public DogApiGroup Group { get; set; }
    }

    public class DogApiGroup
    {
        public DogApiGroupData Data { get; set; }
    }

    public class DogApiGroupData
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }

    public class DogApiMeta
    {
        public DogApiPagination Pagination { get; set; }
    }

    public class DogApiPagination
    {
        public int Current { get; set; }
        public int Next { get; set; }
        public int Last { get; set; }
        public int Records { get; set; }
    }

    public class DogApiLinks
    {
        public string Self { get; set; }
        public string Current { get; set; }
        public string Next { get; set; }
        public string Last { get; set; }
    }
}