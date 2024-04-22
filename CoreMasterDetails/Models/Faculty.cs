using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CoreMasterDetails.Models
{
    public class Faculty
    {
        public Faculty()
        {
            this.Students = new List<Student>();
        }
        public int ID { get; set; }
        [Required]
        public string FacultyName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? CourseStartDate { get; set; }
        [ValidateNever]
        public string PicPath { get; set; }
        [NotMapped]
        public IFormFile Picture { get; set; }
        [ValidateNever]
        public virtual List<Student> Students { get; set; }
    }
}
