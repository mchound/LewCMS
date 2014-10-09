using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public enum StoreInfoAction
    {
        AddOrUpdate = 0,
        Delete = 1
    }

    public enum ContentStatus
    {
        Working = 0,
        PendingPublish = 1,
        Published = 2,
        Overdue = 3
    }

    public enum ContentTypeSpecifier
    {
        Undefined = 0,
        Page = 1,
        Section = 2,
        GlobalConfig = 3
    }

    public enum ContentVersionSelect
    {
        All = 0,
        Latest = 1
    }

}
