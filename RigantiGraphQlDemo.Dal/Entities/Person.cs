using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RigantiGraphQlDemo.Dal.Entities
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SecretPiggyBankLocation { get; set; }

        public ICollection<Farm> Farms { get; set; }
    }
}