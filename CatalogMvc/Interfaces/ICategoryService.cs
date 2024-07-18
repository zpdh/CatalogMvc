using CatalogMvc.Models;

namespace CatalogMvc.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>?> GetCategoriesAsync();
    Task<CategoryViewModel?> GetCategoryByIdAsync(int id);
    Task<CategoryViewModel?> CreateCategoryAsync(CategoryViewModel category);
    Task<bool> UpdateCategoryAsync(CategoryViewModel category, int id);
    Task<bool> DeleteCategoryAsync(int id);
}