﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuetAPI;
using DuetAPI.Commands;
using DuetControlServer.SPI;
using Code = DuetControlServer.Commands.Code;

namespace DuetControlServer.FileExecution
{
    /// <summary>
    /// Implementation of a macro file
    /// </summary>
    public class MacroFile : BaseFile
    {
        /// <summary>
        /// Default name of the config file
        /// </summary>
        public static readonly string ConfigFile = "config.g";

        /// <summary>
        /// Fallback file if the config file could not be found
        /// </summary>
        public static readonly string ConfigFileFallback = "config.g.bak";

        /// <summary>
        /// Config override as generated by M500
        /// </summary>
        public static readonly string ConfigOverrideFile = "config-override.g";

        /// <summary>
        /// List of macro files being executed
        /// </summary>
        private static readonly List<MacroFile> _macroFiles = new List<MacroFile>();

        /// <summary>
        /// Indicates if a file macro is being done
        /// </summary>
        public static bool DoingMacroFile
        {
            get
            {
                lock (_macroFiles)
                {
                    return _macroFiles.Count != 0;
                }
            }
        }

        /// <summary>
        /// Abort files on the given channel (probably because the firmware requested this)
        /// </summary>
        /// <param name="channel">Channel on which macros are supposed to be cancelled</param>
        /// <returns>If an abortion could be requested</returns>
        public static bool AbortAllFiles(CodeChannel channel)
        {
            bool filesAborted = false;
            lock (_macroFiles)
            {
                foreach (MacroFile file in _macroFiles.ToList())
                {
                    if (file.Channel == channel)
                    {
                        file.Abort();
                        _macroFiles.Remove(file);
                        filesAborted = true;
                    }
                }
            }
            return filesAborted;
        }

        /// <summary>
        /// Abort the last file on the given channel
        /// </summary>
        /// <param name="channel">Channel of the running macro file</param>
        /// <returns>If an abortion could be requested</returns>
        public static bool AbortLastFile(CodeChannel channel)
        {
            lock (_macroFiles)
            {
                foreach (MacroFile file in _macroFiles)
                {
                    if (file.Channel == channel)
                    {
                        file.Abort();
                        _macroFiles.Remove(file);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Whether this file is config.g or config.g.bak
        /// </summary>
        public bool IsConfig { get; set; }

        /// <summary>
        /// Whether this file is config-override.g
        /// </summary>
        public bool IsConfigOverride { get; set; }

        /// <summary>
        /// The queued code which originally started this macro file or null
        /// </summary>
        public QueuedCode StartCode { get; }

        /// <summary>
        /// Create a new macro instance
        /// </summary>
        /// <param name="fileName">Filename of the macro</param>
        /// <param name="channel">Channel to send the codes to</param>
        /// <param name="startCode">Which code is starting this macro file</param>
        public MacroFile(string fileName, CodeChannel channel, QueuedCode startCode) : base(fileName, channel)
        {
            string name = Path.GetFileName(fileName);
            IsConfig = (name == ConfigFile || name == ConfigFileFallback);
            IsConfigOverride = (name == ConfigOverrideFile);
            StartCode = startCode;
            lock (_macroFiles)
            {
                _macroFiles.Add(this);
            }

            Console.WriteLine($"[info] Executing {((startCode == null) ? "system" : "nested")} macro file '{fileName}' on channel {channel}");
        }

        /// <summary>
        /// Print diagnostics of this class
        /// </summary>
        /// <param name="builder">String builder</param>
        public static void Diagnostics(StringBuilder builder)
        {
            lock (_macroFiles)
            {
                foreach (MacroFile file in _macroFiles)
                {
                    builder.AppendLine($"Executing {((file.StartCode != null) ? "system" : "regular")} macro file '{file.FileName}' on channel {file.Channel}");
                }
            }
        }

        /// <summary>
        /// Read another code from the file being executed asynchronously
        /// </summary>
        /// <returns>Next available code or null if the file has ended</returns>
        public override Code ReadCode()
        {
            // Read the next code from the file
            Code result = base.ReadCode();
            if (result != null)
            {
                result.FilePosition = null;
                result.Flags |= CodeFlags.IsFromMacro;
                if (IsConfig) { result.Flags |= CodeFlags.IsFromConfig; }
                if (IsConfigOverride) { result.Flags |= CodeFlags.IsFromConfigOverride; }
                if (StartCode != null) { result.Flags |= CodeFlags.IsNestedMacro; }
                result.SourceConnection = (StartCode != null) ? StartCode.Code.SourceConnection : 0;
                return result;
            }

            // Remove reference to this file again
            lock (_macroFiles)
            {
                _macroFiles.Remove(this);
            }
            return null;
        }
    }
}
