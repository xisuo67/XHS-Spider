using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.IService.DI;
using XHS.Models.XHS.ApiOutputModel;
using XHS.Models.XHS.ApiOutputModel.CreateQrCode;
using XHS.Models.XHS.ApiOutputModel.NodeDetail;
using XHS.Models.XHS.ApiOutputModel.OtherInfo;
using XHS.Models.XHS.ApiOutputModel.Search;
using XHS.Models.XHS.ApiOutputModel.UserPosted;
using XHS.Models.XHS.InputModel;

namespace XHS.IService.XHS
{
    /// <summary>
    /// 小红书接口
    /// </summary>
    public interface IXhsSpiderService: ISingletonDependency
    {
        /// <summary>
        /// 获取用户笔记详情
        /// /api/sns/web/v1/feed?source_note_id=xxxxx
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        Task<XHSBaseApiModel<NodeDetailModel>> GetNodeDetail(string nodeid);

        /// <summary>
        /// 获取当前小红书博主其他信息，包含个人信息等数据
        /// </summary>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        Task<XHSBaseApiModel<OtherInfoModel>> GetOtherInfo(string targetUserId);


        /// <summary>
        /// 获取用户所有笔记 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        Task<List<NoteModel>> GetAllUserNode(string userid);

        /// <summary>
        /// 关键字搜索笔记
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        Task<XHSBaseApiModel<SearchNodesOutPutModel>> SearchNotes(SearchInputModel inputModel);
        /// <summary>
        /// 创建登录二维码
        /// </summary>
        /// <returns></returns>
        Task<QrCodeModel> CreateQrCode();
    }
}
