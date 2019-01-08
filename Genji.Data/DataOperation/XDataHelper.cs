//=====================================================
// Copyright © 2017-2018 xuxuzhaozhao
// All rights reserved
// GUID: 0f8385ab-2c30-48aa-b58e-40314afbcb07
// CLR版本: 4.0.30319.42000
// 命名空间名称: Genji.Data.DataOperation
// 文件名: XDataHelper
// 当前系统时间: 2018/3/1 13:58:44
// 当前登录用户名: xuxuzhaozhao
//======================================================

using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Genji.Data.DataOperation
{
    public class XDataHelper
    {
        public static bool ExcuteNonQuery(string sql, object obj = null)
        {
            using (var con = new MySqlConnection(XConfig.MySqlConnectionString))
            {
                return con.Execute(sql, obj) > 0;
            }
        }

        public static T ExcuteScalar<T>(string sql, object obj = null)
        {
            using (var con = new MySqlConnection(XConfig.MySqlConnectionString))
            {
                return con.ExecuteScalar<T>(sql, obj);
            }
        }

        public static IEnumerable<T> ExcuteReader<T>(string sql, object obj = null)
        {
            using (var con = new MySqlConnection(XConfig.MySqlConnectionString))
            {
                return con.Query<T>(sql, obj);
            }
        }
    }
}
