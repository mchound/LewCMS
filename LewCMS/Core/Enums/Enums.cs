using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Enums
{
    public enum ContentStatus
    {
        Working = 0,
        PendingPublish = 1,
        Published = 2,
        Overdue = 3
    }

    public enum ContentVersionSelect
    {
        All = 0,
        Latest = 1
    }

    public enum PageInfoAction
    {
        AddOrUpdate = 0,
        Delete = 1
    }
}
