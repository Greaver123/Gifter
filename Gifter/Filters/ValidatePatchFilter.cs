using Gifter.Services.Common;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Gifter.Filters { 

    /// <summary>
    /// Validation filter for patch requets.
    /// </summary>
    /// <typeparam name="TModel">Type of model for JsonPatchDocument</typeparam>
    public class ValidatePatchFilter<TModel>: IActionFilter where TModel: class 
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            JsonPatchDocument<TModel> patchDocument = null;
            
            foreach (var arg in context.ActionArguments)
            {
                if(arg.Value is JsonPatchDocument<TModel>) patchDocument = arg.Value as JsonPatchDocument<TModel>;
            }

            if (patchDocument == null) return;

            var counter = 0;
            var operationResult = new OperationResult<object>() { 
                Message = "Invalid patch request body.", 
                Status = Gifter.Services.Constants.OperationStatus.FAIL};
            var isValid = true;

            foreach (var operation in patchDocument.Operations)
            {
                var key = $"[{counter}]_{operation.OperationType}";
                var errorsList = new List<string>();

                switch (operation.OperationType)
                {
                    case OperationType.Replace:
                    case OperationType.Add:
                    case OperationType.Test:
                        if (operation.value == null) errorsList.Add($"\'{nameof(operation.value)}\' cannot be null or empty.");
                        if (operation.path == null) errorsList.Add($"\'{nameof(operation.path)}\' cannot be null or empty.");
                        break;
                    case OperationType.Move:
                    case OperationType.Copy:
                        if (operation.path == null) errorsList.Add($"\'{nameof(operation.path)}\' cannot be null or empty.");
                        if (string.IsNullOrWhiteSpace(operation.from)) errorsList.Add($"\'{nameof(operation.from)}\' cannot be null or empty.");
                        break;
                    case OperationType.Invalid:
                        errorsList.Add($"Invalid operation type.");
                        break;
                }

                if(errorsList.Count > 0)
                {
                    operationResult.Errors.Add(key, errorsList);
                    isValid = false;
                }
                counter++;
            }

            if (!isValid) context.Result = new BadRequestObjectResult(operationResult);
        }
    }
}
