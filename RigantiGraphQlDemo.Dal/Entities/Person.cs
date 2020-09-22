using RigantiGraphQlDemo.Dal.Entities.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RigantiGraphQlDemo.Dal.Entities
{
    public class Person : EntityBase
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(200)]
        public string? SecretPiggyBankLocation { get; set; }

        public virtual ICollection<Farm> Farms { get; set; } = default!;
    }
}