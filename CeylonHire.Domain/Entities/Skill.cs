using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public int Id { get; set; }
        public string SkillName { get; set; }
    }
}
