using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.XHS.ApiOutputModel;
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
    }
}
