using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.IService.DI
{

    /// <summary>
    /// 实现该接口将自动注册到Ioc容器，生命周期为单例
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }
}
