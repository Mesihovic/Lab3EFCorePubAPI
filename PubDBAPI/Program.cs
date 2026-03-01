using Microsoft.EntityFrameworkCore;
using PublisherData;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<PubContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PubConnection"))
);


builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();