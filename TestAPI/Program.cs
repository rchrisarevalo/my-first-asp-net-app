using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// Create a builder instance to allow for services
// to be used and to allow for the creation of an
// ASP.NET app variable.
var builder = WebApplication.CreateBuilder(args);

// Allows the viewing of API endpoints via the browser.
builder.Services.AddEndpointsApiExplorer();

// Adds CORS policy.
builder.Services.AddCors(options =>
{
    // Add policy setting origins that the API can
    // be accessed from, as well as determine
    // whether to allow credentials, any header methods,
    // or any HTTP methods, to be used.
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configures the port number that will be used
// for local development.
builder.WebHost.UseUrls(new[]
{
    "http://localhost:5000"
});

// Build the ASP.NET application.
var app = builder.Build();

// If the app is under development, allow
// for the use of CORS.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

// Set up the index route.
app.MapGet("/", () =>
{
    return new { message = "This route is working!" };
});

// Set up a test route performing simple
// additions.
app.MapGet("/add_route", () => {
    int a = 10;
    int b = 20;
    int c = a + b;

    var data = new {
        message = $"The addition of {a} and {b} is {c}."
    };

    return data;
});

app.MapGet("/subtract_route", () =>
{
    int a = 20;
    int b = 15;
    int c = a - b;

    return new { message = $"The subtraction of {a} and {b} is {c}." };
});

app.MapGet("/multiply_route", () =>
{
    int a = 20;
    int b = 20;
    int c = a * b;

    return new { message = $"The multiplication of {a} and {b} is {c}." };
});

app.MapGet("/division_route", () =>
{
    // Add a try-catch block to catch exception
    // where you cannot divide by zero.
    try
    {
        // This will obviously cause an error.
        int a = 20;
        int b = 0;
        int c = a / b;

        return Results.Ok(new { message = $"The division of {a} and {b} is {c}." });
    } 
    catch (DivideByZeroException)
    {
        return Results.Problem(detail: "You cannot divide by zero!");
    }
});

app.Run();
