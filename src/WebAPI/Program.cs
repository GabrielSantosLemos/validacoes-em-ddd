using WebAPI._Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();