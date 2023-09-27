using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RabbitMQ.Client;
using System.Text;
using webform.Models;

namespace webform.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
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
            Countries = GetCountriesList()
        };
    }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid || FormData == null || FormData.SelectedCountryId == 0)
        {
            FormData!.Countries = GetCountriesList();
            return Page();
        }

        var countries = GetCountriesList();

        var book = (bool)FormData.Book! ? "Booked" : "Cancelled";
        var country = countries.FirstOrDefault(x => x.Value == FormData.SelectedCountryId.ToString())!.Text;

        // _logger.LogInformation($"FormData values: {FormData.Name}, {FormData.Email}, {FormData.SelectedCountryId}, {FormData.Book}, Output: {book}, {country}");

        SendRabbitMQMessage(book, country, FormData.Name!, FormData.Email!);
        // Redirect or return a view
        return RedirectToPage("Index"); // You can redirect to the same page or another page.
    }

    private static void SendRabbitMQMessage(string? book, string country, string name, string email)
    {
        var factory = new ConnectionFactory { HostName = "localhost", VirtualHost = "ucl" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "Tour", type: ExchangeType.Topic);

        var routingKey = book != null ? "Tour." + book : "anonymous.info";
        var message = book != null ? $"A tour to {country} from the user \"{name} ({email})\" has been {book}" : "No tour booked";
        var body = Encoding.UTF8.GetBytes(message);
        
        // Start transaction https://blog.rabbitmq.com/posts/2011/02/introducing-publisher-confirms#guaranteed-delivery-with-tx
        channel.TxSelect();
        for (int i = 0; i < 500; i++)
        {
            channel.BasicPublish(exchange: "Tour", routingKey: routingKey, basicProperties: null, body: body);
        }
        channel.TxCommit();
    }
    private List<SelectListItem> GetCountriesList() 
    {
        var selectList = new List<SelectListItem>
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
        };
        return selectList.OrderBy(x => x.Text).ToList();
    }
}
