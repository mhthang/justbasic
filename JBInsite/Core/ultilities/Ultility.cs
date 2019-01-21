using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Core
{
    public partial class Ultility
    {
        /// <summary>
        /// Check Object Value
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public static object COnull(object st)
        {
            object s1 = "";
            if (st == null || st.ToString() == "undefined" || st.ToString().Trim() == "" || st.ToString() == "null" || st.ToString() == "&nbsp;")
            {
                s1 = null;
            }
            else if (st.ToString() == "True" || st.ToString() == "true")
            {
                s1 = "1";
            }
            else if (st.ToString() == "False" || st.ToString() == "false")
            {
                s1 = "0";
            }
            else
            {
                s1 = st;
            }
            return s1;
        }

        public static object Converts(params object[] array)
        {
            if (array != null)
            {
                Dictionary<string, object> ret = new Dictionary<string, object>();
                for (int i = 0; i < array.Length; i++)
                {
                    ret.Add(array[i].ToString(), array[++i]);
                }
                return ret;
            }
            return null;
        }

        public static List<T> ConvertDataTableToList<T>(DataTable table) where T : class, new()
        {
            List<T> listObj = new List<T>();
            T obj = new T();
            if (table == null || table.Rows.Count <= 0) return default(List<T>);
            foreach (DataRow myrow in table.Rows)
            {
                listObj.Add(SetObject<T>(myrow));
            }
            return listObj;
        }

        private static T SetObject<T>(DataRow myRow) where T : class, new()
        {
            T obj = new T();
            obj = ChangeType<T>(obj);
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                obj.GetType().GetProperty(prop.Name).SetValue(obj, (myRow.Table.Columns.Contains(prop.Name) && myRow[prop.Name] != DBNull.Value) ? Convert.ChangeType(myRow[prop.Name], Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) : null, null);
            }
            return obj;
        }

        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value, t);
        }

        public static string VNCurrencyToString(decimal number)
        {
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = " ";
            bool booAm = false;
            decimal decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDecimal(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            return str + "đồng chẵn";
        }

        public static string VNCurrencyToString(double number)
        {
            string s = number.ToString("#");
            string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
            int i, j, donvi, chuc, tram;
            string str = " ";
            bool booAm = false;
            double decS = 0;
            //Tung addnew
            try
            {
                decS = Convert.ToDouble(s.ToString());
            }
            catch
            {
            }
            if (decS < 0)
            {
                decS = -decS;
                s = decS.ToString();
                booAm = true;
            }
            i = s.Length;
            if (i == 0)
                str = so[0] + str;
            else
            {
                j = 0;
                while (i > 0)
                {
                    donvi = Convert.ToInt32(s.Substring(i - 1, 1));
                    i--;
                    if (i > 0)
                        chuc = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        chuc = -1;
                    i--;
                    if (i > 0)
                        tram = Convert.ToInt32(s.Substring(i - 1, 1));
                    else
                        tram = -1;
                    i--;
                    if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
                        str = hang[j] + str;
                    j++;
                    if (j > 3) j = 1;
                    if ((donvi == 1) && (chuc > 1))
                        str = "một " + str;
                    else
                    {
                        if ((donvi == 5) && (chuc > 0))
                            str = "lăm " + str;
                        else if (donvi > 0)
                            str = so[donvi] + " " + str;
                    }
                    if (chuc < 0)
                        break;
                    else
                    {
                        if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
                        if (chuc == 1) str = "mười " + str;
                        if (chuc > 1) str = so[chuc] + " mươi " + str;
                    }
                    if (tram < 0) break;
                    else
                    {
                        if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
                    }
                    str = " " + str;
                }
            }
            if (booAm) str = "Âm " + str;
            return str + "đồng chẵn";
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }

    public static class MethodHelper
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetMethodName(string strClassName)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }

        public static string GetMethodName()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().DeclaringType.FullName + "-" + sf.GetMethod().Name;
        }
    }

    public class UtilityHelper
    {
        public static void WriteLog(Exception ex, string ProjectName, string Method, string Title, string UserName = "")
        {
            try
            {
                //new sys.sysErrorLogBUS().Add(ProjectName, Method, Title,ex.Message, UserName);
            }
            catch (Exception)
            {
            }
        }
    }

    public static class KeyCacheHelper
    {
        public const int _5Minutes = 5;
        public const int _15Minutes = 15;
        public const int _30Minutes = 30;
        public const int _60Minutes = 60;

        public static string GetKeyCache(string projectName, string methodName, string suffix)
        {
            return projectName + "_" + methodName + (!string.IsNullOrEmpty(suffix) ? ("_" + suffix) : suffix);
        }

        public static string GenerateCacheSearch(string strSearchKey, string strRootCacheKey, ref List<string> strKeys)
        {
            string result = "";
            bool flag = false;
            //Xóa dấu tiếng Việt
            string strTemp = strSearchKey;
            for (int i = strKeys.Count - 1; i >= 0; i--)
            {
                if (strTemp.Contains(strKeys[i]))
                {
                    result = strKeys[i];
                    flag = true;
                    break;
                }
            }
            //Có rồi thì không lưu thêm key cache nữa
            if (!flag && !string.IsNullOrEmpty(strTemp))
            {
                strKeys.Add(strTemp);
            }
            return result;
        }
    }
}