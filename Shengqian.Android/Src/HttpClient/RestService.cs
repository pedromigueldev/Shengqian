
using System.Diagnostics;
using System.Text.Json;
using Shengqian.Domain.Dto;

namespace Shengqian.Android.Http;

public class CategoriesService(HttpClient client) : IRestService<CategoryGetAllDto>
{
    public Task DeleteTodoItemAsync(string id) {
        throw new NotImplementedException();
    }

    public async Task<List<CategoryGetAllDto>> RefreshDataAsync()
    {
        var uri = new Uri(client.BaseAddress!, "Categories");
        try
        {
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize(content, SourceGenerationContext.Default.ListCategoryGetAllDto) ?? throw new Exception("Deserialize returned null");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"\tERROR {uri} gave exeption ==> {ex}");
        }

        return [];
    }

    public Task SaveTodoItemAsync(CategoryGetAllDto item, bool isNewItem) {
        throw new NotImplementedException();
    }
}