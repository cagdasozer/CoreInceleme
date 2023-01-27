using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace CoreInceleme.Models
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string City { get; set; }
        public int Salary { get; set; }
    }
}
