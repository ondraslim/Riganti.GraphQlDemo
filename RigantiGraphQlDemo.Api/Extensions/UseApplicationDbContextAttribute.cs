using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using RigantiGraphQlDemo.Dal;
using System.Reflection;

namespace RigantiGraphQlDemo.Api.Extensions
{
    public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<AnimalFarmDbContext>();
        }
    }
}