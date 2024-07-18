using CatalogMvc.Interfaces;
using CatalogMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogMvc.Controllers;

public class CategoriesController(ICategoryService categoryService) : Controller
{
    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Index()
    {
        var result = await categoryService.GetCategoriesAsync();
        
        return (result is null) ? View("Error") : View(result);
    }

    public IActionResult CreateNewCategory()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<ActionResult<CategoryViewModel>> CreateNewCategory(CategoryViewModel category)
    {
        if (ModelState.IsValid)
        {
            var result = await categoryService.CreateCategoryAsync(category); 
            if (result is not null) return RedirectToAction(nameof(Index));
        }
        ViewBag.Error = "Error while creating new category";
        return View(category);
    }
    
    [HttpGet]
    public async Task<IActionResult> UpdateCategory(int id)
    {
        var result = await categoryService.GetCategoryByIdAsync(id);
        return (result is null) ? View("Error") : View(result);
    }
    
    public async Task<IActionResult> UpdateCategory(CategoryViewModel category, int id)
    {
        if (ModelState.IsValid)
        {
            var result = await categoryService.UpdateCategoryAsync(category, id);
            if (result) return RedirectToAction(nameof(Index));
        }
        ViewBag.Error = "Error while updating category";
        return View(category);
    }
    
    [HttpGet]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var result = await categoryService.GetCategoryByIdAsync(id);
        return (result is null) ? View("Error") : View(result);
    }
    
    [ActionName("DeleteCategory")]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        if (ModelState.IsValid)
        {
            var result = await categoryService.DeleteCategoryAsync(id);
            if (result) return RedirectToAction(nameof(Index));
        }
        ViewBag.Error = "Error while deleting category";
        return View("DeleteCategory");
    }
}