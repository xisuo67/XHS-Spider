using Newtonsoft.Json;
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
using XHS.Models.XHS.ApiOutputModel.UserPosted;
using XHS.Models.XHS.InputModel;
using XHS.Service.Log;
using static Microsoft.ClearScript.V8.V8CpuProfile;

namespace XHS.Service.XHS
{
    /// <summary>
    /// 小红书服务
    /// </summary>
    public class XhsSpiderService : IXhsSpiderService
    {
        private static readonly ILogger Logger = LoggerService.Get(typeof(XhsSpiderService));
        /// <summary>
        /// 获取笔记详情
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public XHSBaseApiModel<NodeDetailModel> GetNodeDetail(string nodeid)
        {
            XHSBaseApiModel<NodeDetailModel> nodeDetailModel = new XHSBaseApiModel<NodeDetailModel>();
            try
            {
                string url = $"/api/sns/web/v1/feed?source_note_id={nodeid}";
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoPost(url);
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
        public XHSBaseApiModel<OtherInfoModel> GetOtherInfo(string targetUserId)
        {
            XHSBaseApiModel<OtherInfoModel> model = new XHSBaseApiModel<OtherInfoModel>();
            try
            {
                string url = $"/api/sns/web/v1/user/otherinfo?target_user_id={targetUserId}";
                Logger.Info($"调用接口：{url}");
                var result = HttpClientHelper.DoGet(url);
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
        /// 分页查询用户所有笔记接口
        /// TODO:这里后面迭代要改掉，先测试接口是否能调用成功
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private void UserPosted(UserPostedInputModel model, List<NoteModel> userNodes)
        {
            if (model != null)
            {
                try
                {
                    string url = $"/api/sns/web/v1/user_posted?num={model.num}&cursor={model.cursor}&user_id={model.user_id}";
                    Logger.Info($"调用接口：{url}");
                    var result = HttpClientHelper.DoGet(url);
                    if (!string.IsNullOrEmpty(result))
                    {
                        var resultData = JsonConvert.DeserializeObject<XHSBaseApiModel<UserPostedModel>>(result);
                        if (resultData.Success)
                        {
                            //TODO:cookie请求多次后，会导致触发反爬机制，导致接口返回has_mode字段明明没有更多条，却返回存在多条记录，然后一直递归查询直到标识为false。所以下面需要过滤重复数据，避免添加辣鸡数据
                            foreach (var item in resultData.Data.Notes)
                            {
                                if (!userNodes.Exists(e=>e.NoteId==item.NoteId))
                                {
                                    userNodes.Add(item);
                                }
                            }
                            if (resultData.Data.HasMore)
                            {
                                model.cursor = resultData.Data.Cursor;
                                UserPosted(model, userNodes);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("分页查询用户所有笔记接口失败", ex);
                }
            }
        }
        /// <summary>
        /// 获取用户所有笔记
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<NoteModel> GetAllUserNode(string userid)
        {
            List<NoteModel> nodes = new List<NoteModel>();
            UserPostedInputModel model = new UserPostedInputModel { 
                user_id= userid,
                num=30,
            };
            UserPosted(model,nodes);
            return nodes;
        }
    }
}
