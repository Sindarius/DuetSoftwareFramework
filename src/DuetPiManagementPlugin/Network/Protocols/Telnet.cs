﻿using DuetAPI.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DuetPiManagementPlugin.Network.Protocols
{
    /// <summary>
    /// Protocol management for Telnet
    /// </summary>
    public static class Telnet
    {
        /// <summary>
        /// Initialize the protocol configuration
        /// </summary>
        /// <returns>Asynchronous task</returns>
        public static async Task Init()
        {
            if (File.Exists("/etc/inetd.conf") && await Command.ExecQuery("/usr/bin/systemctl", "is-enabled -q inetd.service"))
            {
                // Although inetd may be used for other purposes, we don't explicitly check if the telnet option is enabled or not
                await Manager.SetProtocol(NetworkProtocol.Telnet, true);
            }
        }

        /// <summary>
        /// Configure the Telnet server
        /// </summary>
        /// <param name="enabled">Enable Telnet</param>
        /// <param name="port">Port</param>
        /// <returns>Configuration result</returns>
        public static async Task<Message> Configure(bool? enabled, int? port)
        {
            if (port != null)
            {
                return new Message(MessageType.Error, "Changing the Telnet port requires manual configuration of inetd");
            }

            // Enable Telnet
            if (enabled.Value && !Manager.EnabledProtocols.Contains(NetworkProtocol.Telnet))
            {
                string startOutput = await Command.Execute("/usr/bin/systemctl", "start inetd.service");
                string enableOutput = await Command.Execute("/usr/bin/systemctl", "enable inetd.service");
                await Manager.SetProtocol(NetworkProtocol.Telnet, true);
                return new Message(MessageType.Success, string.Join('\n', startOutput.TrimEnd(), enableOutput).TrimEnd());
            }

            // Disable Telnet
            if (!enabled.Value && Manager.EnabledProtocols.Contains(NetworkProtocol.Telnet))
            {
                string stopOutput = await Command.Execute("/usr/bin/systemctl", "stop inetd.service");
                string disableOutput = await Command.Execute("/usr/bin/systemctl", "disable inetd.service");
                await Manager.SetProtocol(NetworkProtocol.Telnet, false);
                return new Message(MessageType.Success, string.Join('\n', stopOutput.TrimEnd(), disableOutput).TrimEnd());
            }

            // Don't do anything
            return new Message();
        }

        /// <summary>
        /// Report the current state of the Telnet protocols
        /// </summary>
        /// <param name="builder">String builder</param>
        public static void Report(StringBuilder builder)
        {
            if (Manager.EnabledProtocols.Contains(NetworkProtocol.Telnet))
            {
                builder.AppendLine("Telnet is enabled on port 23");
            }
            else
            {
                builder.AppendLine("Telnet is disabled");
            }
        }
    }
}
