using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.Domain.BaseEntity;

namespace ZamiuxFixer.Domain.Users
{
    public class Role : BaseEntites
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(300)]
        public string RoleName { get; set; }

        [Required]
        [MaxLength(300)]
        public string RoleTitle { get; set; }

        #region Relation
        // One Role mitavanad Many User dashte bashad: (One TO Many)
        public List<User>? Users { get; set; }
        #endregion


    }
}
