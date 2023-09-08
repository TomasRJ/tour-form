using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using webform.Models;

namespace webform.Pages;

public class IndexModel : PageModel
{
    public Form? FormData { get; set; } = default!;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        FormData = new Form // lav select listen
        {
            SelectedCountryId = 0,
            Countries = new List<SelectListItem>
            {
                new SelectListItem{ Value = "1", Text = "Danmark"},
                new SelectListItem{ Value = "2", Text = "Sverige"},
                new SelectListItem{ Value = "3", Text = "Norge"},
                new SelectListItem{ Value = "4", Text = "Finland"},
                new SelectListItem{ Value = "5", Text = "Island"},
                new SelectListItem{ Value = "6", Text = "Færøerne"},
                new SelectListItem{ Value = "7", Text = "Grønland"},
                new SelectListItem{ Value = "8", Text = "Tyskland"},
                new SelectListItem{ Value = "9", Text = "England"},
                new SelectListItem{ Value = "10", Text = "Frankrig"}
            }
        };
    }
}
