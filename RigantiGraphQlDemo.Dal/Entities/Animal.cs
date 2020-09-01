using HotChocolate.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RigantiGraphQlDemo.Dal.Entities
{
    public class Animal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }

        public int FarmId { get; set; }

        [UseSelection]
        public virtual Farm Farm { get; set; }
    }
}