using FluentValidation;
using System.Diagnostics;

namespace Puissance4.API.DTO
{
    public class ValidationErrorDTO
    {
        public int Status => 400;
        public string Title => "One or more validation errors occurred.";
        public string? TraceId { get; }
        public Dictionary<string, string[]>? Errors { get; }

        public ValidationErrorDTO(ValidationException ex)
        {
            TraceId = Activity.Current?.Id;
            if (ex.Errors.Any())
            {
                Errors = ex.Errors.GroupBy(f => f.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());
            }

        }
    }
}
