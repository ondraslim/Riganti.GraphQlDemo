namespace RigantiGraphQlDemo.Api.Exceptions
{
    public class UserError
    {
        public string Message { get; }

        public string Code { get; }

        public UserError(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
}