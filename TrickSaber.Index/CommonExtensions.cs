using TrickSaber.Index;

namespace TrickSaber
{
    public static class CommonExtensions
    {
        public static void Log(this object message)
        {
            Plugin.Log.Debug(message.ToString());
        }

        public static string GetVersionString(this SemVer.Version version)
        {
            return version.Major + "." + version.Minor + "." + version.Patch;
        }
    }
}