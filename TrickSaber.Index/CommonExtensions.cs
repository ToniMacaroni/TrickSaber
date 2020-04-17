namespace TrickSaber.Index
{
    public static class CommonExtensions
    {
        public static void Log(this object message)
        {
            Plugin.Log.Debug(message.ToString());
        }
    }
}