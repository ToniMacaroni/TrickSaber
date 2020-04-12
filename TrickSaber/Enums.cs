using System;

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

    public enum SpinDir
    {
        Forward,
        Backward
    }

    public enum TrickAction
    {
        None,
        Throw,
        Spin
    }

    public static class EnumTools
    {
        public static TrickAction GetTrickAction(this string name)
        {
            return (TrickAction) Enum.Parse(typeof(TrickAction), name, true);
        }
    }
}