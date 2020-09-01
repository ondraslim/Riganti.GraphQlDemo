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
        public int PersonId { get; set; }


        public virtual Person Person { get; set; }

        public virtual ICollection<Animal> Animals { get; set; }
    }
}