using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI;

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

public class ModelStateValidator
{
    public static IActionResult ValidateModelState(ActionContext context)
    {
        (string fieldName, ModelStateEntry entry) = context.ModelState
            .First(x => x.Value.Errors.Count > 0);
        string errorSerialized = entry.Errors.First().ErrorMessage;

        Error error = Error.Deserialize(errorSerialized);
        //Envelope envelope = Envelope.Error(error, fieldName);
        var result = new BadRequestObjectResult(/*envelope*/ $"{error}, {fieldName} || gdsfds");

        return result;
    }
}