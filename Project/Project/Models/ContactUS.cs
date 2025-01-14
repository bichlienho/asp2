using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
	public class ContactUs
	{
        [Key]
        public int Id { get; set; } // ID tự tăng


        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone number must start with 0 and contain exactly 10 digits.")]
        public string Phone { get; set; } // Số điện thoại


        [MinLength(2, ErrorMessage = "Name must contain at least 2 characters.")]

        public string Name { get; set; } // Tên


        [MinLength(3, ErrorMessage = "Comment must contain at least 3 words.")]
        public string Comment { get; set; } // Bình luận


        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } // Email

        public bool Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}