using Gifter.Services.Common;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace Gifter.Filters
{
    public class ValidatePatchFilter<TModel>: IActionFilter where TModel: class 
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var patchDocument = context.ActionArguments["patchWishDocument"] as JsonPatchDocument<TModel>;
            if (patchDocument == null) return;

            foreach (var operation in patchDocument.Operations)
            {
                var errors = new List<string>();
                if (string.IsNullOrWhiteSpace(operation.op)) errors.Add(nameof(operation.op));
                if (string.IsNullOrWhiteSpace(operation.path)) errors.Add(nameof(operation.path));

                switch (operation.OperationType)
                {
                    case OperationType.Replace:
                    case OperationType.Add:
                    case OperationType.Test:
                        if (operation.value == null) errors.Add(nameof(operation.value));
                        break;
                    case OperationType.Move:
                    case OperationType.Copy:
                        if (string.IsNullOrWhiteSpace(operation.from)) errors.Add(nameof(operation.from));
                        break;
                }


                var errorMessage = "Invalid patch request body. ";
                foreach (var prop in errors)
                {
                    errorMessage += $"\'{prop}\' cannot be null or empty.\n";
                }

                if (errors.Count > 0)
                {
                    var operationResult = new OperationResult<object>();
                    operationResult.Message = errorMessage;
                    operationResult.Status = Gifter.Services.Constants.OperationStatus.FAIL;

                    context.Result = new BadRequestObjectResult(operationResult);
                }
            }
        }
    }
}
