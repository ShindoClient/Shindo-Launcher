using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shindo_Launcher.Updater;

namespace Shindo_Launcher.Updater
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(VersionInfo))]
    public partial class VersionInfoJsonContext : JsonSerializerContext
    {
    }
}