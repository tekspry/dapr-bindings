using Dapr.Client;
using ecom.notification.application.Notification;
using ecom.notification.infrastructure.Services.Customer;
using ecom.notification.infrastructure.Services.Email;
using ecom.notification.infrastructure.Services.Product;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<INotificationApplication, NotificationApplication>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddControllers().AddDapr(); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"order Dapr port: {Environment.GetEnvironmentVariable("DAPR_HTTP_PORT")}");
builder.Services.AddDaprClient();
// Using the DAPR SDK to create a DaprClient, in stead of fiddling with URI's our selves
builder.Services.AddSingleton<IProductService>(sc =>
    new ProductService(DaprClient.CreateInvokeHttpClient("product")));

builder.Services.AddSingleton<ICustomerService>(sc =>
    new CustomerService(DaprClient.CreateInvokeHttpClient("customer")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCloudEvents();

app.UseHttpsRedirection();

app.UseCors(p => p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
    endpoints.MapControllers();
});

app.Run();
