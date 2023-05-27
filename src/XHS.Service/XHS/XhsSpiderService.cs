using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Common.Http;
using XHS.IService.XHS;
using XHS.Models.XHS.ApiOutputModel;
using XHS.Models.XHS.ApiOutputModel.NodeDetail;
using XHS.Models.XHS.ApiOutputModel.OtherInfo;
using XHS.Models.XHS.ApiOutputModel.Search;
using XHS.Models.XHS.ApiOutputModel.UserPosted;
using XHS.Models.XHS.InputModel;
using XHS.Service.Log;

namespace XHS.Service.XHS
{
    /// <summary>
    /// 小红书服务
    /// </summary>
    public class XhsSpiderService : IXhsSpiderService
    {
        private static readonly ILogger Logger = LoggerService.Get(typeof(XhsSpiderService));
        private async Task<Dictionary<string,string>> GetXsHeader(string url, WebView2 webView)
        {
            Dictionary<string,string> dic= new Dictionary<string,string>(); 
            string jscode = "var url='" + url + "';\r\n" + @"try {
                                                                            sign(url);
                                                                        } catch (e) { winning.log(e); }
                                                                        function sign(url) {
                                                                            var t;
                                                                            var o = window._webmsxyw(url, t);
                                                                            return o;
                                                                        }";
            var xsxtStr = await webView.CoreWebView2.ExecuteScriptAsync(jscode);
            if (!string.IsNullOrEmpty(xsxtStr)&& xsxtStr!="null")
            {
                try
                {
                    JObject xsxt = (JObject)JsonConvert.DeserializeObject(xsxtStr);
                    var xs = xsxt["X-s"].ToString();
                    var xt = xsxt["X-t"].ToString();
                    dic.TryAdd("X-s", xs);
                    dic.TryAdd("X-t", xt);
                }
                catch (Exception ex)
                {

                    Logger.Error("获取xs，xt算法失败：",ex);
                }
              
            }
            return dic;
        }
        /// <summary>
        /// 获取笔记详情
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<XHSBaseApiModel<NodeDetailModel>> GetNodeDetail(string nodeid, WebView2 webView)
        {
            XHSBaseApiModel<NodeDetailModel> nodeDetailModel = new XHSBaseApiModel<NodeDetailModel>();
            try
            {
                string url = $"/api/sns/web/v1/feed?source_note_id={nodeid}";
                var header=await GetXsHeader(url,webView);
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoPost(url, header);
                if (!string.IsNullOrEmpty(result))
                {
                    nodeDetailModel = JsonConvert.DeserializeObject<XHSBaseApiModel<NodeDetailModel>>(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("获取笔记详细信息失败",ex);
            }
            return nodeDetailModel;
        }
        /// <summary>
        /// 获取当前小红书博主其他信息，包含个人信息等数据
        /// </summary>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<XHSBaseApiModel<OtherInfoModel>> GetOtherInfo(string targetUserId, WebView2 webView)
        {
            XHSBaseApiModel<OtherInfoModel> model = new XHSBaseApiModel<OtherInfoModel>();
            try
            {
                string url = $"/api/sns/web/v1/user/otherinfo?target_user_id={targetUserId}";
                var header = await GetXsHeader(url, webView);
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoGet(url, header);
                if (!string.IsNullOrEmpty(result))
                {
                    model = JsonConvert.DeserializeObject<XHSBaseApiModel<OtherInfoModel>>(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("获取个人信息等失败", ex);
            }
            return model;
        }

        private async Task<bool> UserPosted(UserPostedInputModel model, List<NoteModel> userNodes, WebView2 webView)
        {
            bool isSuccess = false;
            if (model != null)
            {
                try
                {
                    string url = $"/api/sns/web/v1/user_posted?num={model.num}&cursor={model.cursor}&user_id={model.user_id}";
                    var header = await GetXsHeader(url, webView);
                    Logger.Info($"调用接口：{url}");
                    var result = HttpClientHelper.DoGet(url, header);
                    if (!string.IsNullOrEmpty(result))
                    {
                        var resultData = JsonConvert.DeserializeObject<XHSBaseApiModel<UserPostedModel>>(result);
                        if (resultData.Success)
                        {
                            //TODO:cookie请求多次后，会导致触发反爬机制，导致接口返回has_mode字段明明没有更多条，却返回存在多条记录，然后一直递归查询直到标识为false。所以下面需要过滤重复数据，避免添加辣鸡数据
                            foreach (var item in resultData.Data.Notes)
                            {
                                if (!userNodes.Exists(e => e.NoteId == item.NoteId))
                                {
                                    userNodes.Add(item);
                                }
                            }
                            if (resultData.Data.HasMore)
                            {
                                model.cursor = resultData.Data.Cursor;
                                //await Task.Delay(500);
                                await UserPosted(model, userNodes, webView);
                            }
                            isSuccess = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("分页查询用户所有笔记接口失败", ex);
                }
            }
            return isSuccess;
        }
        /// <summary>
        /// 获取用户所有笔记
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<NoteModel>> GetAllUserNode(string userid, WebView2 webView)
        {
            List<NoteModel> nodes = new List<NoteModel>();
            UserPostedInputModel model = new UserPostedInputModel { 
                user_id= userid,
                num=30,
            };
            await UserPosted(model,nodes, webView);
            return nodes;
        }
        /// <summary>
        /// 关键字搜索笔记
        /// </summary>
        /// <param name="inputModel"></param>
        /// <param name="webView"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<XHSBaseApiModel<SearchNodesOutPutModel>> SearchNotes(SearchInputModel inputModel, WebView2 webView)
        {
            XHSBaseApiModel<SearchNodesOutPutModel> model = new XHSBaseApiModel<SearchNodesOutPutModel>();
            try
            {
                string url = $"/api/sns/web/v1/search/notes?keyword={inputModel.KeyWord}&note_type={inputModel.NoteType}&page={inputModel.Page}&page_size={inputModel.PageSize}&search_id={inputModel.SearchId}&sort={inputModel.Sort}";
                var header = await GetXsHeader(url, webView);
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoPost(url, header);
                if (!string.IsNullOrEmpty(result))
                {
                    model = JsonConvert.DeserializeObject<XHSBaseApiModel<SearchNodesOutPutModel>>(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("获取笔记详细信息失败", ex);
            }
            return model;
        }
    }
}
