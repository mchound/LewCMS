using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Content
{
    public enum ContentStatus
    {
        Working = 0,
        PendingPublish = 1,
        Published = 2,
        Overdue = 3
    }
}
