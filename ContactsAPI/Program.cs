using ContactsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { 
        Version = "v1",
        Title = "ContactAPI",
        Description = "API for managing contacts and their skills"
    });

    s.EnableAnnotations();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
    s.IncludeXmlComments(xmlPath);
});
builder.Services.AddDbContext<MyDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Contact API V1");
    });
//}


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
