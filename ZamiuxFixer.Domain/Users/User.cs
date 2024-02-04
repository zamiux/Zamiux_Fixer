using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.Domain.BaseEntity;
using ZamiuxFixer.Domain.Questions;

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
        public string? Avatar { get; set; }
        public string? Description { get; set; }
        public string? JobTitle { get; set; }
        public string? Country { get; set; }
        public int ProfileVisit { get; set; }
        public bool IsEmailActive { get; set; }
        public string ActiveCode { get; set; }

        #region Relation
        // One User Mitavanad Only One Role daste bashad

        [ForeignKey("RoleId")] // moshakhas mikoni kodoom feild Fk mishe
        public Role Role { get; set; }

        [InverseProperty("User1")]
        public List<UserFollowing>? UserFollowing1 { get; set; }

        [InverseProperty("User2")]
        public List<UserFollowing>? UserFollowing2 { get; set; }

        public List<Question> Questions { get; set; }
        #endregion

    }
}
