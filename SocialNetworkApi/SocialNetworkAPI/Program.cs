using SocialNetwork.BLL;
using SocialNetwork.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    builder.Services.AddScoped<IUserAccountRepository>(provider =>
        new UserAccountRepository(@"Data Source=.\SQLExpress;Initial Catalog=SocialNetwork;Integrated Security=True;"));

    builder.Services.AddScoped<IFriendRequestRepository>(provider =>
        new FriendRequestRepository(
            @"Data Source=.\SQLExpress;Initial Catalog=SocialNetwork;Integrated Security=True;"));

    builder.Services.AddScoped<IDialogRepository>(provider =>
        new DialogRepository(@"Data Source=.\SQLExpress;Initial Catalog=SocialNetwork;Integrated Security=True;"));

    builder.Services.AddScoped<IUserRepository>(provider =>
        new UserRepository(
            provider.GetRequiredService<IUserAccountRepository>(),
            provider.GetRequiredService<IFriendRequestRepository>(),
            provider.GetRequiredService<IDialogRepository>()
        )
    );



builder.Services.AddScoped<UserAccountLogic>(provider => 
    new UserAccountLogic(provider.GetRequiredService<IUserRepository>()));
builder.Services.AddScoped<UserRequestLogic>(provider => 
    new UserRequestLogic(provider.GetRequiredService<IUserRepository>()));
builder.Services.AddScoped<UserDialogLogic>(provider => 
    new UserDialogLogic(provider.GetRequiredService<IUserRepository>()));


builder.Services.AddAuthorization(); 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication(); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();





