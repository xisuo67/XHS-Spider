using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Mvvm.Contracts;

namespace XHS.Spider.Services
{
    public interface IPageServiceNew: IPageService
    {
        // 这个用做特定的服务范围
        IServiceScope? Scope { get; set; }
    }
}
