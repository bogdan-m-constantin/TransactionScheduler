using TransactionScheduling.Project.Domain;
using TransactionScheduling.Project.Domain.SQL;
using TransactionScheduling.Project.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSql(builder.Configuration);
builder.Services.AddSingleton<WaitForGraph>();
builder.Services.AddSingleton<TransactionSchedulerService>();
builder.Services.AddSingleton<TransactionSchedulerHostedService>();
builder.Services.AddHostedService<TransactionSchedulerHostedService>();
builder.Services.AddCors((options) =>
{
    options.AddPolicy("cors", (cors) => cors.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("cors");
app.UseAuthorization();

app.MapControllers();

app.Run();
