using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.Models.Events;
using XHS.Models.XHS.ApiOutputModel.CreateQrCode;

namespace XHS.Common.Events.Model
{
    /// <summary>
    /// 二维码状态回调
    /// </summary>
    public class GetQrCodeStatusCallbackEvent : PubSubEvent<QrCodeModel>
    {
    }
}
