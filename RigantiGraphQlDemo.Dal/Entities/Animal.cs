using RigantiGraphQlDemo.Dal.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace RigantiGraphQlDemo.Dal.Entities
{
    public class Animal : EntityBase
    {

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }


        [StringLength(200)]
        public string? Species { get; set; }
        public int FarmId { get; set; }

        public virtual Farm Farm { get; set; } = default!;
    }
}