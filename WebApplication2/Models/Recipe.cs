using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class Recipe
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Название блюда обязательно")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Название блюда должно быть от 3 до 200 символов")]
    [Display(Name = "Название блюда")]
    public string Title { get; set; }
    [Display(Name = "Описание блюда")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "Укажите ингредиенты")]
    [MinLength(10, ErrorMessage = "Ингредиенты должны содержать минимум 10 символов")]
    [Display(Name = "Ингредиенты")]
    public string Ingredients { get; set; }
    [Required(ErrorMessage = "Опишите шаги приготовления")]
    [MinLength(10, ErrorMessage = "Инструкция должна содержать минимум 10 символов")]
    [Display(Name = "Инструкция")]
    public string CookingSteps { get; set; }
    [Display(Name = "Страна (национальная кухня)")]
    public string? Country {  get; set; }
    [Required(ErrorMessage = "Время приготовления обязательно")]
    [Range(1, 1000, ErrorMessage = "Время готовки должно быть от 1 до 1000 минут")]
    [Display(Name = "Время (мин)")]
    public int CookingTime { get; set; }
    public DateTime CreatedAt { get; set; }
}