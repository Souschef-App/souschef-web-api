using souschef.server.Data;
using souschef.server.Helpers;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<PostGresDBContext>(opt =>
        opt.UseNpgsql(ConnectionHelper.GetConnectionString("")));

builder.Services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<PostGresDBContext>();

//builder.Services.AddScoped<SignInManager<ApplicationUser>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using(var scope = app.Services.CreateScope())
    {
        await DataHelper.ManageDataAsync(scope.ServiceProvider);
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();


app.MapControllers();

app.Run();