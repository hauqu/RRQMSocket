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
using System.Security.Cryptography.X509Certificates;

namespace RRQMSocket
{
    /// <summary>
    /// 客户端Ssl验证
    /// </summary>
    public class ClientSslOption : SslOption
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientSslOption()
        {
            X509Store store = new X509Store(StoreName.Root);
            store.Open(OpenFlags.ReadWrite);
            this.ClientCertificates = store.Certificates;
            store.Close();
        }

        private string targetHost;

        /// <summary>
        /// 目标Host
        /// </summary>
        public string TargetHost
        {
            get => this.targetHost;
            set => this.targetHost = value;
        }

        private X509CertificateCollection clientCertificates;

        /// <summary>
        /// 验证组合
        /// </summary>
        public X509CertificateCollection ClientCertificates
        {
            get => this.clientCertificates;
            set => this.clientCertificates = value;
        }
    }
}