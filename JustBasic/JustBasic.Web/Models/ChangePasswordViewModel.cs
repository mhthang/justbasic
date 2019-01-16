using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JustBasic.Web.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Bạn cần nhập mật khẩu hiện tại.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập mật khẩu mới.")]
        [StringLength(100, ErrorMessage = "{0} ít nhất {2} và không quá {1} kí tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu sai.")]
        public string ConfirmPassword { get; set; }
    }
}