using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using WatsonsCase.Application.Consumer;
using WatsonsCase.Application.Services;
using WatsonsCase.Application.Services.Implementations;
using WatsonsCase.Infrastructure;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddEnvironmentVariables()
    .Build();


var redisConnection = ConnectionMultiplexer.Connect("localhost");
builder.Services.AddSingleton(redisConnection);
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("portal-gateway", new OpenApiInfo { Title = "WS Portal Gateway", Version = "v1" });
});
var rabbitMqUri = new Uri("rabbitmq://localhost");
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BasketItemAddedConsumer>();
    x.AddConsumer<BasketItemSendConsumer>();
    x.AddConsumer<BasketItemDeleteConsumer>();
    x.AddConsumer<BasketItemGetConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqUri, h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("basket_delete_queue", e =>
        {
            e.ConfigureConsumer<BasketItemDeleteConsumer>(context);
        });
        cfg.ReceiveEndpoint("basket_added_queue", e =>
        {
            e.ConfigureConsumer<BasketItemAddedConsumer>(context);
        });
        cfg.ReceiveEndpoint("basket_get_queue", e =>
        {
            e.ConfigureConsumer<BasketItemGetConsumer>(context);
        });
        cfg.ReceiveEndpoint("basket_send_queue", e =>
        {
            e.ConfigureConsumer<BasketItemSendConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger(options =>
{
#if !DEBUG
                options.PreSerializeFilters.Add((document, request) =>
                {
                    var openApiPaths = new OpenApiPaths();
                    foreach (var (key, value) in document.Paths)
                    {
                        openApiPaths.Add($"/portal-gateway{key}", value);
                    }
                    document.Paths = openApiPaths;
                });
#endif
});
app.UseSwaggerUI(options =>
{

    var wsPortalGatewayEndpointUrl = "/swagger/portal-gateway/swagger.json";
#if !DEBUG
                wsPortalGatewayEndpointUrl = $"/wsp-portal-gateway-{wsPortalGatewayEndpointUrl}";
#endif
    options.SwaggerEndpoint(wsPortalGatewayEndpointUrl, "WS Portal Gateway");


});
//RecurringJob.AddOrUpdate<Watsons ServiceJob>("Watsons  job1", x => x.GetWatsons s(), Cron.Daily(08, 0), TimeZoneInfo.Local);

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
//BackgroundJob.Enqueue<IMediator>(mediator => mediator.Send(new GetWatsons sApiAll()));


