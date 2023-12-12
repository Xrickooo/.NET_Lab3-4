using SocialNetwork.BLL;
using SocialNetwork.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Регистрация UserRepository в DI контейнере
builder.Services.AddScoped<UserRepository>(_ => new UserRepository(@"Data Source=.\SQLExpress;Initial Catalog=SocialNetwork;Integrated Security=True;"));

// Регистрация UserLogic в DI контейнере
builder.Services.AddScoped<UserLogic>();

 

// Other service configurations...
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();