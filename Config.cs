// Config.cs
using Exiled.API.Interfaces;

namespace SuperCola
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public byte MaxSpeedLevel { get; set; } = 3;
    }
}
