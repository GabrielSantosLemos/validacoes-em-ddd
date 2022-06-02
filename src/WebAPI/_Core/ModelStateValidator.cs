using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebAPI._Core
{
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
}
