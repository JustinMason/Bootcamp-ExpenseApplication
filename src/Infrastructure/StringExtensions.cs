using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Infrastructure
{
    public static class StringExtensions
    {
        public static string FormatWith(this string format, params object[] arguments )
        {
            return String.Format(format, arguments);
        }
    }
}
