﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMSocket.FileTransfer
{
    [Serializable]
    internal class TransferSetting
    {
        internal bool breakpointResume;
        internal int bufferLength;
    }
}