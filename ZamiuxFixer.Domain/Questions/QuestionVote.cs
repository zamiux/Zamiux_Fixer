using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.Domain.Questions
{
    public class QuestionVote
    {
        [Key]
        public int VoteId { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public bool Vote { get; set; } // True: Like, False: Dislike

        #region Relations
        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        #endregion

    }
}
