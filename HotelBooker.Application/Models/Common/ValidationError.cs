namespace HotelBooker.Application.Models.Common
{
    public class ValidationError : Error
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationError(IDictionary<string, string[]> errors)
            : base(ErrorType.Validation, "One or more validation errors occured.")
        {
            Errors = errors;
        }
    }
}
