using System.ComponentModel.DataAnnotations;

namespace BDSA2020.Entities
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public Degree Degree { get; set; }
    }
}