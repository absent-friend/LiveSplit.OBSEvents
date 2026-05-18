using LiveSplit.Options;
using System;

namespace LiveSplit.OBSEvents.Utility
{
    internal class Logger
    {
        private readonly static Action<string> EMPTY_CONSUMER = _ => { };

        private static Action<string> sendToErrorConsumers = EMPTY_CONSUMER;
        private static Action<string> sendToWarningConsumers = EMPTY_CONSUMER;
        private static Action<string> sendToInfoConsumers = EMPTY_CONSUMER;

        private static string AddComponentPrefix(string message)
        {
            return $"[LiveSplit.OBSEvents] {message}";
        }

        public static void AddErrorConsumer(Action<string> consumer)
        {
            if (consumer == EMPTY_CONSUMER)
            {
                sendToErrorConsumers = consumer;
            }
            else
            {
                sendToErrorConsumers += consumer;
            }
        }

        public static void AddWarningConsumer(Action<string> consumer)
        {
            if (consumer == EMPTY_CONSUMER)
            {
                sendToWarningConsumers = consumer;
            }
            else
            {
                sendToWarningConsumers += consumer;
            }
        }

        public static void AddInfoConsumer(Action<string> consumer)
        {
            if (consumer == EMPTY_CONSUMER)
            {
                sendToInfoConsumers = consumer;
            }
            else
            {
                sendToInfoConsumers += consumer;
            }
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
