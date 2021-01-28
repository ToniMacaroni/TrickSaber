using System;

namespace TrickSaber
{
    public enum VRSystem
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

    public enum TrickState
    {
        Inactive,
        Started,
        Ending
    }

    public static class EnumTools
    {
        public static TEnum GetEnumValue<TEnum>(this string name)
        {
            return (TEnum) Enum.Parse(typeof(TEnum), name, true);
        }
    }
}