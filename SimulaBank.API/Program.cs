using SimulaBank.Application.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);

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
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Aponta para o JSON correto
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simula Bank v1");
        // Deixa o Swagger na raiz (http://localhost:5232/)
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();