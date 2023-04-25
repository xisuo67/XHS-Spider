using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XHS.IService.DI;

namespace XHS.IService
{
    public interface IWindowService: IScopeDependency
    {
        public void Show(Type windowType);

        public T Show<T>() where T : class;
    }
}
