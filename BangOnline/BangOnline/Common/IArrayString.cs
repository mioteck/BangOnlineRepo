using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BangOnline.Common
{
    public interface IArrayString
    {
        string[] ToArrayString(bool hideInformation = true);
        string[] BaseInfo();
    }
}
