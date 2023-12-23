using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.Domain.BaseEntity;

namespace ZamiuxFixer.Domain.Users
{
    public class User : BaseEntites
    {
        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [Required]
        [MaxLength(300)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(300)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(100)]
        public string? Mobile { get; set; }

        [Required]
        [MaxLength(300)]
        public string Password { get; set; }
        public bool IsEmailActive { get; set; }

        #region Relation
        // One User Mitavanad Only One Role daste bashad

        [ForeignKey("RoleId")] // moshakhas mikoni kodoom feild Fk mishe
        public Role Role { get; set; }
        #endregion

    }
}
