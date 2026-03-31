using Microsoft.OpenApi.Models;
using SimulaBank.Application.DependencyInjection;
using SimulaBank.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("SimulaBankPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL do seu Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfraestructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Simula Bank",
        Version = "v1",
        Description = "API que irá simular o core de operações bancárias, como cofrinhos, transações, e entre outros."
    });
    // Configuração de segurança para JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no campo abaixo, deverá ser um bearer token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simula Bank v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("SimulaBankPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();