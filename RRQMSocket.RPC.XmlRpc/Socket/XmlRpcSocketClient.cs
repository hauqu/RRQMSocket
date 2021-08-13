﻿using RRQMCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket.RPC.XmlRpc
{
    /// <summary>
    /// XmlRpc辅助类
    /// </summary>
    public class XmlRpcSocketClient: SimpleSocketClient
    {
        /// <summary>
        /// 禁用适配器赋值
        /// </summary>
        /// <param name="adapter"></param>
        public sealed override void SetDataHandlingAdapter(DataHandlingAdapter adapter)
        {
            throw new RRQMException($"{nameof(XmlRpcSocketClient)}不允许设置适配器。");
        }

        internal void SetAdapter(DataHandlingAdapter adapter)
        {
            base.SetDataHandlingAdapter(adapter);
        }
    }
}
