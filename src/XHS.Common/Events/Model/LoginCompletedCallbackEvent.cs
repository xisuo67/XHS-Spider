using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.Events;
using XHS.Models.XHS.ApiOutputModel.Me;

namespace XHS.Common.Events.Model
{
    /// <summary>
    /// 登录成功回调事件
    /// </summary>
    public class LoginCompletedCallbackEvent : PubSubEvent<UserInfoModel>
    {
    }
}
