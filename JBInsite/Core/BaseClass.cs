using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BaseClass
    {
        public const string strProjectName = "justbasic";
        /// <summary>
        /// Ghi log của function
        /// </summary>
        /// <param name="strModule"></param>
        /// <param name="strFunction"></param>
        /// <returns></returns>
        public static string InitLog(string strModule, string strFunction)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(strModule + " - ");
            strBuilder.Append(strFunction);
            return strBuilder.ToString();
        }
    }
}
