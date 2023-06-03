using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using XHS.Common.Global;
using XHS.Common.Http;
using XHS.IService.XHS;
using XHS.Models.XHS.ApiOutputModel;
using XHS.Models.XHS.ApiOutputModel.CreateQrCode;
using XHS.Models.XHS.ApiOutputModel.Login;
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
        #region 公共方法
        private static readonly ILogger Logger = LoggerService.Get(typeof(XhsSpiderService));
        private async Task<Dictionary<string, string>> GetXsHeader(string url, string jsonData = "")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string param = string.Empty;
            if (string.IsNullOrEmpty(jsonData))
            {
                param = "var url='" + url + "';\r\n  var data=null;";
            }
            else
            {
                param = "var url='" + url + "';\r\n var jsonStr='" + jsonData + "';var data = JSON.parse(jsonStr);";
            }

            string jscode = param + @"try {
                                        sign(url,data);
                                    } catch (e) { winning.log(e); }
                                    function sign(url,data) {
                                        var t;
                                        var o = window._webmsxyw(url, t);
                                        return o;
                                    }";
            var xsxtStr = await GlobalCaChe.webView.CoreWebView2.ExecuteScriptAsync(jscode);
            if (!string.IsNullOrEmpty(xsxtStr) && xsxtStr != "null")
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

                    Logger.Error("获取xs，xt算法失败：", ex);
                }

            }
            return dic;
        }
        #endregion

        #region 笔记相关
        /// <summary>
        /// 获取笔记详情
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<XHSBaseApiModel<NodeDetailModel>> GetNodeDetail(string nodeid)
        {
            XHSBaseApiModel<NodeDetailModel> nodeDetailModel = new XHSBaseApiModel<NodeDetailModel>();
            try
            {
                string url = $"/api/sns/web/v1/feed?source_note_id={nodeid}";
                var header = await GetXsHeader(url);
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoPost(url, header);
                if (!string.IsNullOrEmpty(result))
                {
                    nodeDetailModel = JsonConvert.DeserializeObject<XHSBaseApiModel<NodeDetailModel>>(result);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("获取笔记详细信息失败", ex);
            }
            return nodeDetailModel;
        }
        /// <summary>
        /// 获取当前小红书博主其他信息，包含个人信息等数据
        /// </summary>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<XHSBaseApiModel<OtherInfoModel>> GetOtherInfo(string targetUserId)
        {
            XHSBaseApiModel<OtherInfoModel> model = new XHSBaseApiModel<OtherInfoModel>();
            try
            {
                string url = $"/api/sns/web/v1/user/otherinfo?target_user_id={targetUserId}";
                var header = await GetXsHeader(url);
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
        /// <summary>
        /// 递归获取用户笔记
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userNodes"></param>
        /// <returns></returns>
        private async Task<bool> UserPosted(UserPostedInputModel model, List<NoteModel> userNodes)
        {
            bool isSuccess = false;
            if (model != null)
            {
                try
                {
                    string url = $"/api/sns/web/v1/user_posted?num={model.num}&cursor={model.cursor}&user_id={model.user_id}";
                    var header = await GetXsHeader(url);
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
                                await UserPosted(model, userNodes);
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
        public async Task<List<NoteModel>> GetAllUserNode(string userid)
        {
            List<NoteModel> nodes = new List<NoteModel>();
            UserPostedInputModel model = new UserPostedInputModel
            {
                user_id = userid,
                num = 30,
            };
            await UserPosted(model, nodes);
            return nodes;
        }
        #endregion



        #region 关键字搜索
        /// <summary>
        /// 关键字搜索笔记
        /// </summary>
        /// <param name="inputModel"></param>
        /// <param name="webView"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<XHSBaseApiModel<SearchNodesOutPutModel>> SearchNotes(SearchInputModel inputModel)
        {
            XHSBaseApiModel<SearchNodesOutPutModel> model = new XHSBaseApiModel<SearchNodesOutPutModel>();
            try
            {
                //string url = $"/api/sns/web/v1/search/notes?keyword={inputModel.KeyWord}&note_type={inputModel.NoteType}&page={inputModel.Page}&page_size={inputModel.PageSize}&search_id={inputModel.SearchId}&sort={inputModel.Sort}";
                string url = "/api/sns/web/v1/search/notes";
                var postData = JsonConvert.SerializeObject(inputModel);
                var header = await GetXsHeader(url, postData);
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoPost(url, header, postData);
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
        #endregion

        #region 登录
        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<QrCodeModel> CreateQrCode()
        {
            QrCodeModel model =null;
            try
            {
                string url = "/api/sns/web/v1/login/qrcode/create";
                var header = await GetXsHeader(url);
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoPost(url, header);
                if (!string.IsNullOrEmpty(result))
                {
                    var qrcodeModel = JsonConvert.DeserializeObject<XHSBaseApiModel<QrCodeModel>>(result);
                    if (qrcodeModel != null && qrcodeModel.Success)
                    {
                        model = qrcodeModel.Data;
                    }
                    else
                    {
                        Logger.Error("获取二维码信息失败:" + qrcodeModel.Msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("获取二维码信息失败", ex);
            }

            return model;
        }
        /// <summary>
        /// 获取登录状态
        /// </summary>
        /// <param name="qrCode"></param>
        /// <returns></returns>
        public async Task<LoginInfoStatus> GetStatus(QrCodeModel qrCode)
        {
            LoginInfoStatus status = null;
            try
            {
                string url = $"/api/sns/web/v1/login/qrcode/status?qr_id={qrCode.QrId}&code={qrCode.Code}";
                var header = await GetXsHeader(url);
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoGet(url, header);
                if (!string.IsNullOrEmpty(result))
                {
                    var loginStatus = JsonConvert.DeserializeObject<XHSBaseApiModel<LoginInfoStatus>>(result);
                    if (loginStatus != null && loginStatus.Success && loginStatus.Data.CodeStatus==2)
                    {
                        status = loginStatus.Data;
                        return status;
                    }
                    else
                    {
                        await Task.Delay(1000);
                        //递归
                        GetStatus(qrCode).GetAwaiter();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("获取二维码信息失败", ex);
            }
            return status;
        }


        #endregion

    }
}
