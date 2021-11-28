using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RollingRetention.Models
{

    public class UserActivity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter User ID")]
        [Display(Name = "UserID")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Enter Date")]
        [Display(Name = "Date Registration")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Enter Date")]
        [Display(Name = "Date Last Activity")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastActivityDate { get; set; }


    }
}
