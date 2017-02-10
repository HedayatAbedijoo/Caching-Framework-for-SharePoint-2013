using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caching
{
    public class ExpireCacheAttribute : System.Attribute
    {                
        public Type[] Regions { get; set; }
    }

   
}
