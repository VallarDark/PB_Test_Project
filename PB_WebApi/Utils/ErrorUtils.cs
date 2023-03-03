using Domain.Exceptions;

namespace PB_WebApi.Utils
{
    public static class ErrorUtils
    {
        private const string SOME_SERVER_ERROR = "Some internal server error was happened";

        public static IResult Handle(this Exception ex)
        {
            if (ex is EmptyCollectionException
                    || ex is InvalidLoginDataException
                    || ex is InvalidTokenException
                    || ex is ItemAlreadyExistsException
                    || ex is ItemNotExistsException
                    || ex is LowPrevilegiesLevelException
                    || ex is NullValueException
                    || ex is WrongCollectionItemsCountException
                    || ex is WrongOperationException
                    || ex is ArgumentException)
            {
                return Results.Problem(ex.Message);
            }

            return Results.Problem(SOME_SERVER_ERROR);
        }
    }
}
