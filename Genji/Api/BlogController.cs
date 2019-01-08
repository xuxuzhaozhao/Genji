using Genji.Data.Common;
using Genji.Data.DataOperation;
using Genji.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Genji.Api
{
    [Produces("application/json")]
    [Route("api/Blog")]
    public class BlogController : Controller
    {
        [Route("getone")]
        [HttpGet]
        public object Get(int id)
        {
            var result = new XResult();

            var select = $@"SELECT * FROM BlogArticle WHERE ID = @id;
                            UPDATE BlogArticle SET ReadCount = (ReadCount+1) WHERE ID = @id";
            var one = XDataHelper.ExcuteReader<BlogArticle>(select, new { id }).SingleOrDefault();
            result.Data = one;
            return result;
        }

        [HttpPost]
        public object Add([FromBody]BlogArticle model)
        {
            var result = new XResult();

            var insert = $@"INSERT INTO BlogArticle (Title,Image,Category,Tag,Content,ReadCount,CreateTime,ModifiedTime)VALUES (@Title,@Image,@Category,@Tag,@Content,0,Now(),Now())";

            var whereObj = new { Category = model.Category, Image = model.Image, Tag = model.Tag, Title = model.Title, Content = model.Content };
            if (XDataHelper.ExcuteNonQuery(insert, whereObj))
            {
                return result;
            }
            result.Code = -1;
            result.Message = "操作失败";
            return result;
        }

        [HttpPut("{id}")]
        public object Update(int id, [FromBody]BlogArticle model)
        {
            var result = new XResult();

            var update = $@"UPDATE BlogArticle SET 
                                   Title = @Title,
                                   Image=@Image,
                                   Category = @Category,
                                   Tag = @Tag,
                                   Content = @Content,
                                   ModifiedTime = Now()
                            WHERE ID = @id";

            var whereObj = new { id = id, Image = model.Image, Category = model.Category, Tag = model.Tag, Title = model.Title, Content = model.Content };
            if (XDataHelper.ExcuteNonQuery(update, whereObj))
            {
                return result;
            }
            result.Code = -1;
            result.Message = "操作失败";
            return result;
        }

        [Route("addLikeCount")]
        [HttpGet("{id}")]
        public object AddLikeCount(int id)
        {
            var result = new XResult();

            var update = $@"UPDATE BlogArticle SET LikeCount = (LikeCount+1) WHERE ID = @id";

            if (XDataHelper.ExcuteNonQuery(update, new { id }))
            {
                return result;
            }
            result.Code = -1;
            result.Message = "操作失败";
            return result;
        }

        [Route("publishArticle")]
        [HttpGet("{id}")]
        public object Publish(int id)
        {
            var result = new XResult();

            var update = $@"UPDATE BlogArticle SET IsPublished = 1 WHERE ID = @id";

            if (XDataHelper.ExcuteNonQuery(update, new { id }))
            {
                return result;
            }
            result.Code = -1;
            result.Message = "操作失败";
            return result;
        }

        [HttpDelete("{id}")]
        public object Delete(int id)
        {
            var result = new XResult();
            var delete = $"UPDATE BlogArticle SET DELETETIME=Now(), IsDeleted=1 WHERE ID = @id";

            if (XDataHelper.ExcuteNonQuery(delete, new { id }))
            {
                return result;
            }
            result.Code = -1;
            result.Message = "操作失败";
            return result;
        }

        [Route("getlist")]
        [HttpPost]
        public object GetList([FromBody]XPagination page)
        {
            var whereSql = $"WHERE {(page.IsBackAdmin ? "" : "IsPublished = 1 AND")} IsDeleted=0 AND {page.WhereTags} AND {page.WhereCategory} AND {page.WhereTitle}";
            var selectSql = $@"select * from VM_BlogArticle {whereSql}
                            ORDER BY Id DESC
                            LIMIT {(page.Page - 1) * page.RowsPerPage},{page.RowsPerPage}";
            var countSql = $"SELECT COUNT(1) AS count FROM BlogArticle {whereSql}";

            var list = XDataHelper.ExcuteReader<BlogArticle>(selectSql).ToList();

            var count = XDataHelper.ExcuteScalar<int>(countSql);

            var result = new XResult();
            result.Data = new
            {
                Content = list,
                Count = count
            };
            return result;
        }

        [HttpPost, Route("advice")]
        public object Advice([FromBody]XAdvice advice)
        {
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();

            var sql = "INSERT INTO BlogAdvice(ArticleId,Email,NickName,Advice,CreateTime,IPAddress) VALUES (@ArticleId,@Email,@NickName,@Advice,NOW(),@IPAddress)";

            var result = new XResult();
            if (XDataHelper.ExcuteNonQuery(sql, new
            {
                ArticleId = advice.ArticleId,
                Email = advice.Email,
                NickName = advice.NickName,
                Advice = advice.Advice,
                IPAddress = ip
            }))
            {
                return result;
            }
            result.Code = -1;
            result.Message = "哦豁,添加失败!";
            return result;
        }
    }
    public class XAdvice
    {
        public int ArticleId { get; set; } = 0;
        public string Email { get; set; }
        public string NickName { get; set; } = "无名氏";
        public string Advice { get; set; }
    }
    public class XPagination
    {
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; } = 1;
        [JsonProperty(PropertyName = "rowsPerPage")]
        public int RowsPerPage { get; set; } = 8;
        /// <summary>
        /// 默认全部
        /// </summary>
        public int Category { get; set; } = 0;
        /// <summary>
        /// 以空格字符相隔
        /// </summary>
        public string Tags { get; set; }

        public string Title { get; set; }

        public bool IsBackAdmin { get; set; } = false;

        [JsonIgnore]
        public string WhereTags
        {
            get
            {
                if (!string.IsNullOrEmpty(Tags))
                {
                    var whereString = "(";
                    var tagList = Tags.Split(' ').ToList();
                    foreach (var item in tagList)
                    {
                        whereString += $"Tags LIKE '%{item.DelSpace()}%' OR";
                    }
                    whereString = whereString.Trim("OR") + ")";
                    return whereString;
                }
                return "1=1";
            }
        }

        /// <summary>
        /// 如果分类大于1,才加入where条件语句
        /// </summary>
        [JsonIgnore]
        public string WhereCategory => Category > 0 ? $"Category={Category}" : "1=1";

        /// <summary>
        /// 如果分类大于1,才加入where条件语句
        /// </summary>
        [JsonIgnore]
        public string WhereTitle => !string.IsNullOrEmpty(Title) ? $"Title LIKE '%{Title.DelSpace()}%'" : "1=1";
    }
}