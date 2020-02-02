using System;
using System.Collections.Generic;

using Audit.objects;

using System.Threading.Tasks;

namespace Audit.interfaces
{
    public interface IHolyDaysService
    {
        IEnumerable<DateTime> HolyWorkDays(DateTime DFrom);
        IEnumerable<DateTime> HolyDays(DateTime DFrom);
    }
}
