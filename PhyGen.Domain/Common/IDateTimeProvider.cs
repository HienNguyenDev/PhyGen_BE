using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhyGen.Domain.Common
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}
