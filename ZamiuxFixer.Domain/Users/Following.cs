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
    public class UserFollowing
    {
        [Key]
        public int FollowingId { get; set; }

        public int Follower { get; set; }
        public int Following { get; set; }

        #region Relation
        [ForeignKey("Follower")]
        public User? User1 { get; set; }

        [ForeignKey("Following")]
        public User? User2 { get; set; } 
        #endregion
    }
}
