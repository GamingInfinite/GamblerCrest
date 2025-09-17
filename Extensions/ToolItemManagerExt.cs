using System.Reflection;

namespace GamblerCrest.Extensions
{
    internal static class ToolItemManagerExt
    {
        public static void AddCrest(this ToolItemManager tim, ToolCrest crest)
        {
            ToolCrestList currentList = (ToolCrestList)typeof(ToolItemManager).GetField("crestList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tim);
            currentList.Add(crest);
            typeof(ToolItemManager).GetField("crestList", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tim, currentList);
        }

        public static ToolCrestList GetCrestList(this ToolItemManager tim)
        {
            return (ToolCrestList)typeof(ToolItemManager).GetField("crestList", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tim);
        }
    }
}
