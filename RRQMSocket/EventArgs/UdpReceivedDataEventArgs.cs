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
using RRQMCore.ByteManager;
using System.Net;

namespace RRQMSocket
{
    /// <summary>
    /// Udp接收消息
    /// </summary>
    public class UdpReceivedDataEventArgs : ReceivedDataEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        public UdpReceivedDataEventArgs(EndPoint endPoint, ByteBlock byteBlock, IRequestInfo requestInfo) : base(byteBlock, requestInfo)
        {
            this.EndPoint = endPoint;
        }

        /// <summary>
        /// 接收终结点
        /// </summary>
        public EndPoint EndPoint { get; }
    }
}
