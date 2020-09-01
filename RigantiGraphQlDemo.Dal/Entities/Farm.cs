using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RigantiGraphQlDemo.Dal.Entities
{
    public class Farm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Species { get; set; }
        public int PersonId { get; set; }


        [UseSelection]
        public virtual Person Person { get; set; }

        [UsePaging]
        [UseSelection]
        [UseFiltering]
        public virtual ICollection<Animal> Animals { get; set; }


    }
}