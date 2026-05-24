using LiveSplit.Options;
using System;

namespace LiveSplit.OBSEvents.Utility
{
    internal class Logger
    {
        private static Action<string> sendToErrorConsumers;
        private static Action<string> sendToWarningConsumers;
        private static Action<string> sendToInfoConsumers;

        private static string AddComponentPrefix(string message)
        {
            return $"[LiveSplit.OBSEvents] {message}";
        }

        public static void AddErrorConsumer(Action<string> consumer)
        {
            sendToErrorConsumers += consumer;
        }

        public static void AddWarningConsumer(Action<string> consumer)
        {
            sendToWarningConsumers += consumer;
        }

        public static void AddInfoConsumer(Action<string> consumer)
        {
            sendToInfoConsumers += consumer;
        }

        public static void Error(string message)
        {
            sendToErrorConsumers(message);
            Log.Error(AddComponentPrefix(message));
        }

        public static void Warning(string message)
        {
            sendToWarningConsumers(message);
            Log.Warning(AddComponentPrefix(message));
        }

        public static void Info(string message)
        {
            sendToInfoConsumers(message);
            Log.Info(AddComponentPrefix(message));
        }
    }
}
