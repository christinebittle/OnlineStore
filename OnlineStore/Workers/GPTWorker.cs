using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Protocol;
using OnlineStore.Data;
using OnlineStore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OnlineStore.Workers
{
    public class GPTWorker : BackgroundService
    {
        private readonly ILogger<GPTWorker> _logger;
        private readonly HttpClient _httpClient;
        private readonly  IServiceProvider _serviceProvider;

        public GPTWorker(ILogger<GPTWorker> logger, IServiceProvider serviceProvider, HttpClient httpClient)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _httpClient = httpClient;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            while (!stoppingToken.IsCancellationRequested)
            {
                // debug line
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // stack overflow - injection of database context to singleton worker
                // https://stackoverflow.com/questions/66617422/adding-dbcontext-service-to-program-cs-in-worker-project
                using var scope = _serviceProvider.CreateScope();
                var services = scope.ServiceProvider;
                var context = services.GetService<AppDbContext>();

                Product Product = context.Products.Where(p=>p.ai_generated==false).FirstOrDefault();

                if (Product != null) { 

                    //setting up the prompt for AI
                    Message message1 = new Message();
                    message1.role = "system";
                    message1.content = "You are a helpful assistant for an online store";

                    Message message2 = new Message();
                    message2.role = "user";
                    message2.content = $"Write a product description for a product with a name {Product.ProductName}";

                    Prompt prompt = new Prompt() { model = "gpt-4o-mini", messages=new List<Message>() {message1, message2} };

                    string jsonpayload = prompt.ToJson().ToString();

                    _logger.LogInformation(jsonpayload);


                    //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    {
                        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer {key-goes-here}");
                    }
                

                    HttpContent content = new StringContent(jsonpayload);
                    content.Headers.ContentType.MediaType = "application/json";


                    // https://platform.openai.com/docs/quickstart?language-preference=curl
                    HttpResponseMessage response = _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content).Result;
                    string responsestring = await response.Content.ReadAsStringAsync();
                    dynamic responsecontent = JObject.Parse(responsestring);

                    _logger.LogInformation(responsestring);

                    string productdesc = responsecontent["choices"][0]["message"]["content"];
                    _logger.LogInformation(productdesc);

                    if (response.IsSuccessStatusCode) { 

                        // fields to update
                        Product.ProductDescription = productdesc;
                        Product.ai_generated = true;

                        // update product 
                        context.Entry(Product).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                    }


                    // NOTES:
                    // This is purely an example of server to server connection - Instructor / Institution is not liable for any generated content from the example
                    // service should really be an independent process outside of the web application
                    // improvements can be made on failed HTTP requests
                    // API token is not committed as part of the repo - you can sign up for your own account 
                    // should be a separate ai api gateway class if we wanted to use AI API for other parts of this project

                }

                // 10s pause for demonstration
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
