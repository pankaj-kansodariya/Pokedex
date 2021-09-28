using System.Net;

namespace Pokedex.API
{
    public static class APIConstants
    {
        // Http Status
        public const int HttpStatusCode_OK = (int)HttpStatusCode.OK;
        public const int HttpStatusCode_BadRequest = (int)HttpStatusCode.BadRequest;
        public const int HttpStatusCode_InternalServerError = (int)HttpStatusCode.InternalServerError;
        public const int HttpStatusCode_NotFound = (int)HttpStatusCode.NotFound;

        public const string ERROR_InvalidInput = "Invalid Input";
        public const string ERROR_UnexpectedError = "Unexpected Error occured - Could not get Pokemon Information";
    }
}
