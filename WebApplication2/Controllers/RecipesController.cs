using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers;

public class RecipesController : Controller
{
    private readonly RecipesDbContext _dbContext;

    public RecipesController(RecipesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // GET: Recipes/ - список всех рецептов (главная менеджера рецептов)
    [HttpGet]
    public async Task<IActionResult> Recipes(string searchString, string recipeCountry)
    {
        // 1. Получаем список всех уникальных категорий для выпадающего списка
        IQueryable<string> countryQuery = _dbContext.Recipes
        .OrderBy(r => r.Country)
        .Select(r => r.Country)
        .Distinct();

        // 2. Создаем базовый запрос на получение рецептов
        var recipes = from r in _dbContext.Recipes
                      select r;

        // 3. Фильтруем по названию, если строка поиска не пуста
        if (!string.IsNullOrEmpty(searchString))
        {
            recipes = recipes.Where(s => s.Title.Contains(searchString));
        }

        // 4. Фильтруем по стране
        if (!string.IsNullOrEmpty(recipeCountry))
        {
            recipes = recipes.Where(x => x.Country == recipeCountry);
        }

        // Передаем список категорий во View через ViewBag, чтобы построить выпадающий список
        ViewBag.Categories = await countryQuery.ToListAsync();

        return View(await recipes.ToListAsync());
    }

    // GET: Recipes/Details/{int id}
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        var recipe = await _dbContext.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound(); // 404
        }

        return View(recipe);
    }
    
    [HttpGet]
    public IActionResult AddRecipe()
    {
        return View();
    }

    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRecipe(Recipe recipe1)
    {
        var recipe = new Recipe
        {
            Title = recipe1.Title,
            Description = recipe1.Description,
            Ingredients = recipe1.Ingredients,
            CookingSteps = recipe1.CookingSteps,
            Country = recipe1.Country,
            CookingTime = recipe1.CookingTime,
            CreatedAt = DateTime.Now
        };
        _dbContext.Recipes.Add(recipe);
        await _dbContext.SaveChangesAsync();
        
        //return Content("Рецепт добвлен");
        return RedirectToAction(nameof(Recipes));

    }

    // GET: Recipes/Edit/{int id}
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        var recipe = await _dbContext.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound(); // 404
        }

        return View(recipe);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Recipe recipe)
    {
        if (id != recipe.Id)
            return NotFound();
        
        if (ModelState.IsValid)
        {
            try
            {
                _dbContext.Update(recipe);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Recipes));
            }
            catch (DbUpdateConcurrencyException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        return View(recipe);
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        var recipe = await _dbContext.Recipes.FindAsync(id);
        if (recipe != null)
        {
            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Recipes));
    }
}