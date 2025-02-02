using Microsoft.EntityFrameworkCore;
using UserAuthAPI.Data; 
using UserAuthAPI.Models; 

var builder = WebApplication.CreateBuilder(args);

// Add database connection
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {

        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
              
    });
});


builder.Services.AddControllers(); // Registers controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configures the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS 
app.UseCors("AllowAngularClient");

app.UseHttpsRedirection();

app.MapControllers(); // Ensures controllers are mapped

app.Run();
