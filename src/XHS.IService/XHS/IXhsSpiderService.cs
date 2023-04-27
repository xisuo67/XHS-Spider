using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.XHS.ApiOutputModel.NodeDetail;
using XHS.Models.XHS.ApiOutputModel.UserPosted;
using XHS.Models.XHS.InputModel;

namespace XHS.IService.XHS
{
    /// <summary>
    /// 小红书接口
    /// </summary>
    public  interface IXhsSpiderService
    {
        /// <summary>
        ///分页查询用户所有笔记接口 /api/sns/web/v1/user_posted
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UserPostedModel UserPosted(UserPostedInputModel model);

        /// <summary>
        /// 获取用户笔记详情
        /// /api/sns/web/v1/feed?source_note_id=xxxxx
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        NodeDetailModel GetNodeDetail(string nodeid);
    }
}
