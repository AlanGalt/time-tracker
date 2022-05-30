using GetThingsDoneAPI.Data;
using GetThingsDoneAPI.Services.ConcreteServices.ProjectService;
using GetThingsDoneAPI.Services.ConcreteServices.IssueService;
using GetThingsDoneAPI.Services.ConcreteServices.TimeEntryService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
    .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.UseDateOnlyTimeOnlyStringConverters());

// Dependency injection
builder.Services.AddDbContext<GetThingsDoneDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IIssueService, IssueService>();
builder.Services.AddTransient<ITimeEntryService, TimeEntryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
