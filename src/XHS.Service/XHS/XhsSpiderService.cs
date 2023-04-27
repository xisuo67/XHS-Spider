using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Common.Http;
using XHS.IService.XHS;
using XHS.Models.XHS.ApiOutputModel.NodeDetail;
using XHS.Models.XHS.ApiOutputModel.OtherInfo;
using XHS.Models.XHS.ApiOutputModel.UserPosted;
using XHS.Models.XHS.InputModel;
using static Microsoft.ClearScript.V8.V8CpuProfile;

namespace XHS.Service.XHS
{
    /// <summary>
    /// 小红书服务
    /// </summary>
    public class XhsSpiderService : IXhsSpiderService
    {
        /// <summary>
        /// 获取笔记详情
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public NodeDetailModel GetNodeDetail(string nodeid)
        {
            NodeDetailModel nodeDetailModel = new NodeDetailModel();
            try
            {
                string url = $"/api/sns/web/v1/feed?source_note_id={nodeid}";
                var result = HttpClientHelper.DoPost(url);
                if (!string.IsNullOrEmpty(result))
                {
                    nodeDetailModel = JsonConvert.DeserializeObject<NodeDetailModel>(result);
                }
            }
            catch (Exception ex)
            {
            }
            return nodeDetailModel;
        }
        /// <summary>
        /// 获取当前小红书博主其他信息，包含个人信息等数据
        /// </summary>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public OtherInfoModel GetOtherInfo(string targetUserId)
        {
            OtherInfoModel model=new OtherInfoModel();
            try
            {
                string url = $"/api/sns/web/v1/user/otherinfo?target_user_id={targetUserId}";
                var result = HttpClientHelper.DoGet(url);
                if (!string.IsNullOrEmpty(result))
                {
                    model = JsonConvert.DeserializeObject<OtherInfoModel>(result);
                }
            }
            catch (Exception ex)
            {
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
        public UserPostedModel UserPosted(UserPostedInputModel model)
        {
            UserPostedModel userPostedModel= new UserPostedModel();

            if (model!=null)
            {
                try
                {
                    string url = $"/api/sns/web/v1/user_posted?num={model.num}&cursor=&user_id={model.user_id}";
                    var result = HttpClientHelper.DoGet(url);
                    if (!string.IsNullOrEmpty(result))
                    {
                        userPostedModel = JsonConvert.DeserializeObject<UserPostedModel>(result);
                    }
                }
                catch (Exception ex)
                {
                }
                
            }
            return userPostedModel;
        }


    }
}
