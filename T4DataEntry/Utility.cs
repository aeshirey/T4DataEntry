using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4DataEntry
{
    public static class Utility
    {
        /// <summary>
        /// LINQ-friendly foreach that permits a final action to be taken
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> inputs, Action<T> action)
        {
            foreach(var input in inputs)
            {
                action(input);
            }
        }
    }
}
