using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text;

namespace AppointmentIngestion.Api.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static ProblemDetails CreateNotFound(
            this ProblemDetailsFactory detailsFactory,
            HttpContext context,
            string? details = null,
            IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status404NotFound, context, details, errors);
        }

        public static ProblemDetails CreateBadRequest(
            this ProblemDetailsFactory detailsFactory,
            HttpContext context,
            string? details = null,
            IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status400BadRequest, context, details, errors);
        }

        public static ProblemDetails CreateConflict(
            this ProblemDetailsFactory detailsFactory,
            HttpContext context,
            string? details = null,
            IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status409Conflict, context, details, errors);
        }

        public static ProblemDetails CreateValidation(
           this ProblemDetailsFactory detailsFactory,
           HttpContext context,
           string? details = null,
            IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status400BadRequest, context, details, errors);
        }

        public static ProblemDetails CreateUnexpectedResponse(
           this ProblemDetailsFactory detailsFactory,
           HttpContext context,
           string? details = null,
           IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status500InternalServerError, context, details, errors);
        }


        private static ProblemDetails CreateProblemDetailsWith(ProblemDetailsFactory detailsFactory, int statusCode,
           HttpContext context,
           string? message = null,
           IEnumerable<string>? errors = null)
        {
            if (errors != null && errors.Any())
            {
                string errorList = new StringBuilder()
                                         .AppendJoin(",", errors)
                                             .ToString();

                return detailsFactory.CreateProblemDetails(context, statusCode: statusCode, detail: errorList);
            }
            else
                return detailsFactory.CreateProblemDetails(context, statusCode: statusCode, detail: message);
        }
    }
}
