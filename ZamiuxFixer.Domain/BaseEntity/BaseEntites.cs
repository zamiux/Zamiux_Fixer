using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZamiuxFixer.Domain.BaseEntity
{
    public class BaseEntites
    {
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
