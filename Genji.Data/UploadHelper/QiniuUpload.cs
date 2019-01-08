//=====================================================
// Copyright © 2017-2018 xuxuzhaozhao
// All rights reserved
// GUID: 5d00c645-3c9e-493c-9a61-1470fdf2079c
// CLR版本: 4.0.30319.42000
// 命名空间名称: Genji.Data.UploadHelper
// 文件名: QiniuUpload
// 当前系统时间: 2018/3/6 16:52:11
// 当前登录用户名: xuxuzhaozhao
//======================================================

using Genji.Data.DataOperation;
using Qiniu.Storage;
using Qiniu.Util;

namespace Genji.Data.UploadHelper
{
    public class QiniuUpload
    {
        public string Token { get; set; }
        public QiniuUpload()
        {
            Mac mac = new Mac(XConfig.AccessKey, XConfig.SecretKey);
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = XConfig.Bucket;
            putPolicy.SetExpires(7200);

            Token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        }
    }
}
