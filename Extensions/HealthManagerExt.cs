using System.Reflection;

namespace GamblerCrest.Extensions
{
    internal static class HealthManagerExt
    {
        public static HealthManager GetSendDamageTo(this HealthManager manager)
        {
            return (HealthManager)typeof(HealthManager).GetField("sendDamageTo", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(manager);
        }
    }
}
