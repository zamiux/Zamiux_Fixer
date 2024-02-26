using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.Domain.BaseEntity;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.Domain.Questions
{
    public class Answer:BaseEntites
    {
        [Key]
        public int AnswerId { get; set; }

        public int UserId { get; set; }

        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsTrueAnswer { get; set; } = false;

        #region Relation
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }
        #endregion
    }
}
