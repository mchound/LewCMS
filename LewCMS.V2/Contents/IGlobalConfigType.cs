using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2.Contents
{
    public interface IGlobalConfigType : IContentType
    {
    }

    public class GlobalConfigType : ContentType, IGlobalConfigType
    {
        
    }
}
