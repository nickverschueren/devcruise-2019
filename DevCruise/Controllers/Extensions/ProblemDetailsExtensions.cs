using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Euricom.DevCruise.Controllers.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static IActionResult Problem(this ControllerBase controller, int statusCode, string fieldName, params string[] errorMessages)
        {
            return controller.StatusCode(statusCode, new FieldProblemDetails(new Dictionary<string, string[]> { { fieldName, errorMessages } })
            {
                Status = statusCode,
                Type = $"https://httpstatuses.com/{statusCode}",
                Title = ReasonPhrases.GetReasonPhrase(statusCode)
            });
        }

        public static IActionResult Problem(this ControllerBase controller, int statusCode, string errorMessage)
        {
            return controller.StatusCode(statusCode, new ProblemDetails
            {
                Status = statusCode,
                Type = $"https://httpstatuses.com/{statusCode}",
                Title = ReasonPhrases.GetReasonPhrase(statusCode),
                Detail = errorMessage
            });
        }

    }

    internal class FieldProblemDetails : ProblemDetails
    {
        public FieldProblemDetails(IDictionary<string, string[]> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            Errors = new Dictionary<string, string[]>(errors, StringComparer.Ordinal);
        }

        [JsonProperty(PropertyName = "errors")]
        public IDictionary<string, string[]> Errors { get; }
    }
}