using System.ComponentModel.DataAnnotations;

namespace CoreInceleme.Models
{
    public class Customer
    {

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }
}
