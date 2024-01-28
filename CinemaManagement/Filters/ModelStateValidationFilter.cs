using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CinemaManagement.Filters
{

    public class ModelStateValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Model state is not valid, throw an exception
                throw new InvalidModelStateException("Invalid model state.");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { 
            // Do nothing on action executed
        }
    }

    // Custom exception for invalid model state
    public class InvalidModelStateException : Exception
    {
        public InvalidModelStateException(string message) : base(message)
        {
        }
    }
}
