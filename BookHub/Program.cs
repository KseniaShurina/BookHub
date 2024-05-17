using BookHub.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.AddDependencies(builder.Configuration);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());



var connectionString1 = builder.Configuration.GetValue<string>("Database:ConnectionString");


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var connectionString2 = app.Configuration.GetConnectionString("ConnectionString");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();