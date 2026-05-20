using LiveSplit.Model;
using System;
using System.Text.RegularExpressions;

namespace LiveSplit.OBSEvents.Utility
{
    internal class ReplayFilenameFormatter
    {
        private static readonly Regex KEYWORD = new("%(?<keyword>game|category|segment|time|splitMS|splitS|splitM|splitH|date|day|month|year)");

        public static string Format(string format, LiveSplitState state, int splitIndex, TimeSpan segmentTime)
        {
            DateTime date = DateTime.Now;
            string formatted = KEYWORD.Replace(format, match =>
                match.Groups["keyword"].Value switch
                {
                    "game" => state.Run.GameName,
                    "category" => state.Run.CategoryName,
                    "segment" => state.Run[splitIndex].Name,
                    "time" => FormatSegmentTime(segmentTime),
                    "splitMS" => $"{segmentTime.Milliseconds:D3}",
                    "splitS" => $"{segmentTime.Seconds:D2}",
                    "splitM" => $"{segmentTime.Minutes:D2}",
                    "splitH" => $"{segmentTime.Hours}",
                    "date" => $"{date:yyyy-MM-dd}",
                    "day" => $"{date.Day}",
                    "month" => $"{date.Month}",
                    "year" => $"{date.Year}",
                    _ => throw new Exception($"Illegal state in {nameof(ReplayFilenameFormatter)}.{nameof(Format)}")
                }
            );
            return FileOperations.StripInvalidFilenameChars(formatted);
        }

        private static string FormatSegmentTime(TimeSpan segmentTime)
        {
            string format;
            if (segmentTime.Hours > 0)
            {
                format = "h'h'mm'm'ss's'fff'ms'";
            }
            else if (segmentTime.Minutes > 0)
            {
                format = "m'm'ss's'fff'ms'";
            }
            else
            {
                format = "s's'fff'ms'";
            }
            return segmentTime.ToString(format);
        }
    }
}
