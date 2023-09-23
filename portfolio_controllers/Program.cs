
using System.Configuration;
using portfolio_business_logic;
using portfolio_website_repo;

namespace portfolio_website;

public class Program
{

    public IConfiguration Configuration { get; }
    public Program(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IRegister, Register>();
        builder.Services.AddScoped<IRepoStringConfig, Register_Repo_Access>();// this interface holds the IConfiguration code access method.
        builder.Services.AddScoped<IRegister_Repo_Access, Register_Repo_Access>();
        builder.Services.AddCors((options) =>
        {
            options.AddPolicy(name: "allowAll", policy1 =>
            {
                // policy1.WithOrigins("http://127.0.0.1:5500", "http://localhost:4200", "http://70.112.56.122")
                // .AllowAnyHeader()
                // .AllowAnyMethod();

                policy1.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("allowAll");
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
