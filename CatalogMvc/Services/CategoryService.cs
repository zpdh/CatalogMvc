using System.Text;
using System.Text.Json;
using CatalogMvc.Interfaces;
using CatalogMvc.Models;

namespace CatalogMvc.Services;

public class CategoryService(IHttpClientFactory clientFactory) : ICategoryService
{
    private const string ApiEndpoint = "/api/v1/categories";
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    private CategoryViewModel? _categoryVm;
    private IEnumerable<CategoryViewModel>? _categoriesVm;

    public async Task<IEnumerable<CategoryViewModel>?> GetCategoriesAsync()
    {
        var client = clientFactory.CreateClient("CategoriesApi");
        using var response = await client.GetAsync(ApiEndpoint);
        if (!response.IsSuccessStatusCode) return null;

        var apiResponse = await response.Content.ReadAsStreamAsync();
        _categoriesVm =
            await JsonSerializer.DeserializeAsync<IEnumerable<CategoryViewModel>>(apiResponse, _options);

        return _categoriesVm;
    }

    public async Task<CategoryViewModel?> GetCategoryByIdAsync(int id)
    {
        var client = clientFactory.CreateClient("CategoriesApi");
        using var response = await client.GetAsync(ApiEndpoint + $"/{id}");

        if (!response.IsSuccessStatusCode) return null;

        var apiResponse = await response.Content.ReadAsStreamAsync();
        _categoryVm = await JsonSerializer.DeserializeAsync<CategoryViewModel>(apiResponse, _options);

        return _categoryVm!;
    }

    public async Task<CategoryViewModel?> CreateCategoryAsync(CategoryViewModel category)
    {
        var client = clientFactory.CreateClient("CategoriesApi");
        
        var categoryAsJson = JsonSerializer.Serialize(category);
        StringContent content = new(categoryAsJson, Encoding.UTF8, "application/json");
        
        using var response = await client.PostAsync(ApiEndpoint, content);
        
        if (!response.IsSuccessStatusCode) return null;
        
        _categoryVm = category;
        
        return _categoryVm;
    }

    public async Task<bool> UpdateCategoryAsync(CategoryViewModel category, int id)
    {
        if (category.CategoryId == 0) category.CategoryId = id;
        var client = clientFactory.CreateClient("CategoriesApi");
        
        using var response = await client.PutAsJsonAsync(ApiEndpoint + $"/{id}", category);
        
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var client = clientFactory.CreateClient("CategoriesApi");
        
        using var response = await client.DeleteAsync(ApiEndpoint + $"/{id}");

        return response.IsSuccessStatusCode;
    }
}