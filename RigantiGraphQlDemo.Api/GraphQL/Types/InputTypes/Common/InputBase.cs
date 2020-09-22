namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common
{
    public class InputBase
    { 
        public string? ClientMutationId { get; }

        public InputBase(string? clientMutationId)
        {
            ClientMutationId = clientMutationId;
        }
    }
}