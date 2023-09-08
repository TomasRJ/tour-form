using Microsoft.AspNetCore.Mvc.Rendering;

namespace webform.Models;

public class Form
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int SelectedCountryId { get; set; }
    public List<SelectListItem>? Countries { get; set; }
    public bool Book { get; set; }
    public bool Cancel { get; set; }
}