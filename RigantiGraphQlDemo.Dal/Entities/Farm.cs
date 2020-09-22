using RigantiGraphQlDemo.Dal.Entities.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RigantiGraphQlDemo.Dal.Entities
{
    public class Farm : EntityBase
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        public int PersonId { get; set; }


        public virtual Person Person { get; set; } = default!;

        public virtual ICollection<Animal> Animals { get; set; } = default!;
    }
}