using Patient.Service.Entities.Context;
using Patient.Service.Repository;
using Patient.Service.Repository.IRepository;
using Patient.Service.Service;
using Patient.Service.Service.IService;
using Patient.Service.Utility;
using WatchDog;
using WatchDog.src.Enums;
using WatchDog.src.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = false;
    //opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Weekly;
    opt.SetExternalDbConnString = builder.Configuration.GetConnectionString("PatientDBConnection");
    opt.DbDriverOption = WatchDogDbDriverEnum.MSSQL;
});
builder.Services.AddSingleton<PatientDbContext>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware(typeof(GlobalErrorHandlingMiddleware));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "Qwerty@123";
});

app.Run();
