using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Genji.Model
{
    public class XResult
    {
        public XResult()
        {
            this.Code = 20000;
            this.Message = "操作成功";
        }
        public XResult(int Code,string Message)
        {
            this.Code = Code;
            this.Message = Message;
        }
        public int Code { get; set; } 
        public object Data { get; set; }
        public string Message { get; set; } 
    }
}
