using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrickSaber
{
    public enum VrSystem
    {
        Oculus,
        SteamVR
    }

    public enum ThumstickDir
    {
        Horizontal,
        Vertical
    }

    public static class EnumTools
    {
        public static ThumstickDir GetDir(string str)
        {
            return (ThumstickDir)Enum.Parse(typeof(ThumstickDir), str, true);
        }
    }
}
