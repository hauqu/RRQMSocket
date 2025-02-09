//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.Dependency;
using System;

namespace RRQMSocket
{
    /// <summary>
    /// 具有插件功能的对象
    /// </summary>
    public interface IPlguinObject
    {
        /// <summary>
        /// 内置IOC容器
        /// </summary>
        IContainer Container { get; }

        /// <summary>
        /// 插件管理器
        /// </summary>
        IPluginsManager PluginsManager { get; }

        /// <summary>
        /// 是否已启用插件
        /// </summary>
        bool UsePlugin { get; }

        /// <summary>
        /// 添加插件
        /// </summary>
        /// <typeparam name="TPlugin">插件类型</typeparam>
        /// <returns>插件类型实例</returns>
        TPlugin AddPlugin<TPlugin>() where TPlugin : IPlugin;

        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="plugin">插件</param>
        /// <exception cref="ArgumentNullException"></exception>
        void AddPlugin(IPlugin plugin);

        /// <summary>
        /// 清空插件
        /// </summary>
        void ClearPlugins();

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="plugin"></param>
        void RemovePlugin(IPlugin plugin);

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RemovePlugin<T>() where T : IPlugin;
    }
}
