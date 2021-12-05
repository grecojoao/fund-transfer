using FundTransfer.IoC;

var builder = WebApplication.CreateBuilder(args);
DependencyInjector.InjectDependencies(builder.Services, builder.Configuration).Wait();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();