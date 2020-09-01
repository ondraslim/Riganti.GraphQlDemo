using GraphQL.Types;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class PersonType : ObjectGraphType<Person>
    {
        public PersonType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
            Field(x => x.Name).Description("The name of the Farm.");
            Field(x => x.Farms, type: typeof(ListGraphType<FarmType>)).Description("The farms of the Person.");

            // We do not want to expose SecretPiggyBankLocation
            //Field(x => x.SecretPiggyBankLocation).Description("The secret location of person's piggy bank. (should not be available!)");
        }
    }
}