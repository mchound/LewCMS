﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.V2
{
    public interface ISectionType : IContentType
    {

    }

    public class SectionType : ContentType, ISectionType
    {

    }
}