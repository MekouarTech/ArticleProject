using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ArticalProject.Core
{
    public class Category
    {
        [Required]
        [Display(Name = "المعرف")]
        public int Id { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [Display(Name = "اسم الصنف")]
        [MaxLength(50,ErrorMessage = "اعلى قيمة للادخال هي 50 حرف")]
        [MinLength(2,ErrorMessage = "ادنى قيمة للادخال هي 2 احرف")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        // Navigation
        public virtual List<AuthorPost> AuthorPosts { get; set; }
    }
}
