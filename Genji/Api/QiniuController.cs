using Genji.Data.UploadHelper;
using Genji.Model;
using Microsoft.AspNetCore.Mvc;

namespace Genji.Api
{
    [Produces("application/json")]
    [Route("api/Qiniu")]
    public class QiniuController : Controller
    {
        [HttpGet]
        public object GetImageToken()
        {
            var result = new XResult();
            QiniuUpload qiniu = new QiniuUpload();
            if (string.IsNullOrEmpty(qiniu.Token))
            {
                result.Code = 10000;
                result.Message = "获取Token失败";
            }
            result.Data = qiniu.Token;
            return result;
        }
    }
}