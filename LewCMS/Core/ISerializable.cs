using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LewCMS.Core
{
    public interface ISerializable
    {
        void SerializeToFile(string filePath);
        //void DeserializeFromFile(string filePath);
    }
}
