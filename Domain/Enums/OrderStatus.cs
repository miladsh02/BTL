using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        InProcess = 0,
        Delivered = 1,
        Cancelled = 2,
        InTransaction=3,
        TransactionFailed=4
    }
}
