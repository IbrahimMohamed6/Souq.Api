
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Souq.Api.Helper.Errors;
using Souq.Api.Helper.Mapping;
using Souq.Core.Entites.Identity;
using Souq.Core.RepositoryContract;
using Souq.Core.Service.Contarct;
using Souq.Infrastructure.Data.DbContexts;
using Souq.Infrastructure.DataSeed.Identity;
using Souq.Infrastructure.Repository;
using Souq.Service;
using Souq.Service.Token;
using Souq.Service.Token.Email;
using StackExchange.Redis;
using System.Text;

namespace Souq.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            #region Configuration Service

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(Genericrepository<>));
            builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
            builder.Services.AddScoped(typeof(IBasketReposatory), typeof(BasketReposatory));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped(typeof(ITokenService), typeof(TokenService));
            builder.Services.AddScoped(typeof(IEmailService), typeof(EmailService));
            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            builder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            builder.Services.AddAutoMapper(typeof(MappingProfils));
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var error = actioncontext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors)
                    .Select(p => p.ErrorMessage)
                    .ToList();

                    var ValiditionErrorResponse = new ApiValiditionError()
                    {
                        Errors = error
                    };
                    return new BadRequestObjectResult(ValiditionErrorResponse);
                };
            });

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(p =>
            {
                var connetion = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connetion);

            });

            builder.Services.AddDbContext<SouqeIdentityDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<SouqeIdentityDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddCors(Options =>
            Options.AddPolicy("MyPolicy", Options =>
            Options.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(builder.Configuration["FrontBaseUrl"]!)
            ));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
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
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
              {
                  options.TokenLifespan = TimeSpan.FromDays(30);  // غيرها إذا كانت قصيرة جدًا
              });


            #endregion

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            #region UpdateDataBase
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            var Logger = LoggerFactory.CreateLogger<Program>();


            try
            {
                var _dbContext = Services.GetRequiredService<StoreContext>();
                var IdentityDb = Services.GetRequiredService<SouqeIdentityDbContext>();
                var userManeger = Services.GetRequiredService<UserManager<AppUser>>();

                await _dbContext.Database.MigrateAsync();
                await IdentityDb.Database.MigrateAsync();
                await DataSeedDbContext.SeedAsync(_dbContext);
                await IdentityDataSeed.SeedUserAsync(userManeger);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An Error Where Applaying Migration");
            }

            #endregion
            // Configure the HTTP request pipeline.
            #region Configuration
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            #endregion

            app.Run();

        }
    }
}
