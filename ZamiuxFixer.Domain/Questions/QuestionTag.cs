using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZamiuxFixer.Domain.Questions
{
    public class QuestionTag
    {
        [Key]
        public int TagId { get; set; }

        public int QuestionId { get; set; }

        [MaxLength(100)]
        public string Tag { get; set; }

        #region Relation
        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }
        #endregion
    }
}
