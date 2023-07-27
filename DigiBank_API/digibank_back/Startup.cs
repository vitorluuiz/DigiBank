using digibank_back.Contexts;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace digibank_back
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddControllers()
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
               });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                                builder =>
                                {
                                    builder.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "digiBank.WebAPI", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });

            services
                .AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = "JwtBearer";
                    option.DefaultChallengeScheme = "JwtBearer";
                }
                )

                .AddJwtBearer("JwtBearer", options =>
                options.TokenValidationParameters = new TokenValidationParameters()
                {

                    // sera validado emissor do token
                    ValidateIssuer = true,

                    //sera validade endereco do token
                    ValidateAudience = true,

                    //sera vailidado tempo do token
                    ValidateLifetime = true,

                    //definicao da chave de seguranca
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("usuario-login-auth")),

                    //define o tempo de expiracao
                    ClockSkew = TimeSpan.FromHours(1),

                    ValidIssuer = "digiBank.WebApi",

                    ValidAudience = "digiBank.WebApi"
                }
                );

            services.AddDbContext<digiBankContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Matheus"))
            );


            services.AddTransient<DbContext, digiBankContext>();
            services.AddTransient<IAvaliacaoRepository, AvaliacaoRepository>();
            services.AddTransient<ICartaoRepository, CartaoRepository>();
            services.AddTransient<ICondicaoRepository, CondicaoRepository>();
            services.AddTransient<ICurtidaRepository, CurtidaRepository>();
            services.AddTransient<IEmprestimoRepository, EmprestimoRepository>();
            services.AddTransient<IEmprestimosOptionsRepository, EmprestimosOptionsRepository>();
            services.AddTransient<IHistoryInvestRepository, HistoryInvestRepository>();
            services.AddTransient<IImgsProdutoRepository, ImgsProdutoRepository>();
            services.AddTransient<IInventarioRepository, InventarioRepository>();
            services.AddTransient<IInvestimentoOptionsRepository, InvestimentoOptionsRepository>();
            services.AddTransient<IInvestimentoRepository, InvestimentoRepository>();
            services.AddTransient<IMarketplaceRepository, MarketplaceRepository>();
            services.AddTransient<IMetasRepository, MetasRepository>();
            services.AddTransient<IPoupancaRepository, PoupancaRepository>();
            services.AddTransient<IRendaFixaRepository, RendaFixaRepository>();
            services.AddTransient<ITipoInvestimentoRepository, TipoInvestimentoRepository>();
            services.AddTransient<ITransacaoRepository, TransacaoRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();

            services.AddTransient<UsuarioRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "digiBank.WebAPI");
                c.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles(); // For the wwwroot folder

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles/Images")),
                RequestPath = "/img"
            });

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
