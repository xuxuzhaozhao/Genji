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
    [Route("api/Category")]
    public class CategoryController : Controller
    {
        [HttpGet]
        [Route("getSelectList")]
        public object Get()
        {
            var optionSql = $"SELECT id AS value,NAME AS label FROM BlogCategory ORDER BY SerialNumber";

            var result = XDataHelper.ExcuteReader<Options>(optionSql).ToList();

            return new XResult() { Data = result };
        }
    }
    public class Options
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }
}