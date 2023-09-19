using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace webform.Models;

public class Form
{
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    public int SelectedCountryId { get; set; }
    public List<SelectListItem>? Countries { get; set; }
    
    [Required(ErrorMessage = "Book is required")]
    public bool? Book { get; set; }
}