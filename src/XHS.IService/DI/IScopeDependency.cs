using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHS.IService.DI
{
    /// <summary>
    /// 当前请求作用域内  只有当前这个实例
    /// </summary>
    public interface IScopeDependency: IDependency
    {
    }
}
