using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakUp.Integrations.Google.Helpers
{
    public class TypeConverter
    {
        private static TypeConverter _instance;
        public static TypeConverter Instance
        {
            get { return _instance ?? new TypeConverter(); }
        }

        public Type GetType(string type)
        {
            return Type.GetType(type);
        }

    }
}
