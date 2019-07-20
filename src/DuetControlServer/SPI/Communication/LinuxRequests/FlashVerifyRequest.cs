﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DuetControlServer.SPI.Communication.LinuxRequests
{
    /// <summary>
    /// Used as the last message to check if the firmware has been flashed successfully
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 8)]
    struct FlashVerifyRequest
    {
        /// <summary>
        /// Length of the flashed firmware
        /// </summary>
        public uint firmwareLength;

        /// <summary>
        /// CRC16 checksum of the firmware binary
        /// </summary>
        public ushort crc16;
    }
}
