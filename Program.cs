using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using Api.Application.Startup;

EnvManager.LoadConfig();

var AllowAnyOrigin = "_allowAnyOrigin";

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetupAppConfig();
await builder.Services.AddAppServices(builder.Configuration);

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: AllowAnyOrigin,
		policy =>
		{
			policy.SetIsOriginAllowed(origin => true)
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		});
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
	app.UseSwagger(c =>
	{
		c.RouteTemplate = "docs/{documentName}/openapi.yaml";
	});
	app.UseSwaggerUI(c =>
	{
		c.DocumentTitle = "My Api";
		c.SwaggerEndpoint("/docs/v1/openapi.yaml", "My Api v1");
		c.RoutePrefix = "docs";
	});
}

app.UseCors(AllowAnyOrigin);
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();
