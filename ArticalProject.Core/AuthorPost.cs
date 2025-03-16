using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticalProject.Core
{
    public class AuthorPost
    {
        [Required]
        [Display(Name = "المعرف")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "معرف المستخدم")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "هذه الخانة مطلوبه")]
        [Display(Name = "الصنف")]
        [DataType(DataType.Text)]
        public string PostCategory { get; set; }

        [Required(ErrorMessage = "هذه الخانة مطلوبه")]
        [Display(Name = "العنوان")]
        [DataType(DataType.Text)]
        public string PostTitle { get; set; }

        [Required(ErrorMessage = "هذه الخانة مطلوبه")]
        [Display(Name = "وصف المنشور")]
        [DataType(DataType.MultilineText)]
        public string PostDecription { get; set; }

        [Required(ErrorMessage = "هذه الخانة مطلوبه")]
        [Display(Name = "الصورة")]
        [DataType(DataType.Upload)]
        public string PostImageURL { get; set; }

        [Display(Name = "تاريخ الاضافة")]
        public DateTime AddedDate { get; set; }

        // Navigation Area
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
