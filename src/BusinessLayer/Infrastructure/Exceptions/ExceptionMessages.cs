namespace BusinessLayer.Infrastructure.Exceptions
{
    public static class ExceptionMessages
    {
        public static string ResourceNotFoundExceptionMessage { get; private set; } = "The resource with specified path was not found";

        public static string ValidatorNotFoundExceptionMessage { get; private set; } = "There were not found any validator to process the request with specified document";
    }
}