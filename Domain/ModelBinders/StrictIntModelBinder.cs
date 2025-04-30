using Microsoft.AspNetCore.Mvc.ModelBinding;



namespace Domain.ModelBinders
{
    public class StrictIntModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            var value = valueProviderResult.FirstValue;

            if (int.TryParse((string?)value, out var intValue) && !IsQuoted(value))
            {
                bindingContext.Result = ModelBindingResult.Success(intValue);
            }
            else
            {
                bindingContext.ModelState.AddModelError(
                    bindingContext.ModelName,
                    "StockQuantity must be a valid integer, not a string.");
            }

            return Task.CompletedTask;
        }

        private bool IsQuoted(string input) =>
            input.StartsWith("\"") && input.EndsWith("\"");
    }
}