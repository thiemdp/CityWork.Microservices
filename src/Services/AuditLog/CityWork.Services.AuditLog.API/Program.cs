using CityWork.Services.AuditLog.API;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCityWorkLogger(configuration =>
{
    return new LoggingOptions();
});
// Add services to the container.
var AppSettings = new AppSettings();
builder.Configuration.Bind(AppSettings);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddModule(AppSettings);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MigrateDatabase();
app.Run();
