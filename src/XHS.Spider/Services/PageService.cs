using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace XHS.Spider.Services
{
    /// <summary>
    /// Service that provides pages for navigation.
    /// </summary>
    public class PageService : IPageServiceNew, IDisposable
    {
        /// <summary>
        /// Service which provides the instances of pages.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates new instance and attaches the <see cref="IServiceProvider"/>.
        /// </summary>
        public PageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        IServiceScope? m_scope = null;
        public IServiceScope? Scope { get => m_scope; set => m_scope = value; }

        public void Dispose()
        {
            m_scope?.Dispose();
        }

        /// <inheritdoc />
        public T? GetPage<T>() where T : class
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(typeof(T)))
                throw new InvalidOperationException("The page should be a WPF control.");

            return (T?)_serviceProvider.GetService(typeof(T));
        }

        /// <inheritdoc />
        public FrameworkElement? GetPage(Type pageType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(pageType))
                throw new InvalidOperationException("The page should be a WPF control.");

            // 在进入创建YourScopedPage之前，检查scope是否存在
            if (pageType == typeof(Views.Pages.UserProfilePage) && m_scope is not null)
            {
                // 如果存在，那就在这个scope内找到这个page类型的实例，如果没有就创建
                return m_scope.ServiceProvider.GetRequiredService(pageType) as FrameworkElement;
            }
            else
            {
                m_scope?.Dispose();
                m_scope = null;
            }

            return _serviceProvider.GetService(pageType) as FrameworkElement;
        }
    }
}
