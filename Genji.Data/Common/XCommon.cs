using System;
using System.Collections.Generic;
using System.Text;

namespace Genji.Data.Common
{
    public static class XCommon
    {
        /// <summary>
        /// 删除空格
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string DelSpace(this string sql)
        {
            return sql?.Replace(" ", "");
        }

        /// <summary>
        /// 去掉字符串S首尾特定字符串R
        /// </summary>
        /// <param name="s"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static string Trim(this string s, string r)
        {
            if (string.IsNullOrEmpty(s)) return s;
            while (s.StartsWith(r))
            {
                s = s.Substring(r.Length);
            }
            while (s.EndsWith(r))
            {
                s = s.Substring(0, s.Length - r.Length);
            }
            return s;
        }
    }
}
