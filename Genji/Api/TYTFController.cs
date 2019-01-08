using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Genji.Data.DataOperation;
using Genji.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Genji.Api
{
    [Produces("application/json")]
    [Route("api/TYTF")]
    public class TYTFController : Controller
    {
        [HttpGet, Route("explores")]
        public object GetMenuList(bool isMenu)
        {
            var sql = isMenu ? "SELECT Id,src,CONCAT(src,msrc) AS msrc,alt,title,w,h FROM TYTFMenus WHERE Id < 11" :
                "SELECT Id,src,CONCAT(src,msrc) AS msrc,alt,title,w,h FROM TYTFMenus WHERE Id > 11";

            var list = XDataHelper.ExcuteReader<object>(sql).ToList();
            return new XResult() { Data = list };
        }

        [HttpGet, Route("head")]
        public object GetMainImage()
        {
            var sql = "SELECT src AS main1,msrc AS main2,CONCAT('http://img.xuxuzhaozhao.top',alt) AS logo FROM TYTFMenus WHERE id = 11";
            var main = XDataHelper.ExcuteReader<object>(sql).SingleOrDefault();
            return new XResult() { Data = main };
        }
    }
}