using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZamiuxFixer.Domain.BaseEntity;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.Domain.Questions
{
    public class Question:BaseEntites
    {
        [Key]
        public int QuestionId { get; set; }
        public int UserId { get; set; }


        [Required]
        [MaxLength(600)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int QuestionVisit { get; set; } = 0;


        #region Relations

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public List<QuestionTag>? QuestionTags { get; set; }

        #endregion
    }
}
