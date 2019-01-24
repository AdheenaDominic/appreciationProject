using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AppreciationCards.Models
{
    public partial class XeroValues
    {
        public XeroValues()
        {
            Messages = new HashSet<Messages>();
        }

        public int ValueId { get; set; }
        public string ValueName { get; set; }

        [IgnoreDataMember]
        public ICollection<Messages> Messages { get; set; }
    }
}
