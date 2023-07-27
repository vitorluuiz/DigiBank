using System.Collections.Generic;

namespace digibank_back.Utils
{
    public class NumberRange
    {
        public int Start { get; set; }
        public int End { get; set; }
    }

    public class NumberMagik
    {
        public static List<int> GetIntRange(int start, int end)
        {
            List<int> result = new List<int>();

            for (int i = start; i <= end; i++)
            {
                result.Add(i);
            }

            return result;
        }

        public static List<int> MergeIntRanges(List<NumberRange> ranges)
        {
            List<int> mergeInts = new();

            foreach (NumberRange range in ranges)
            {
                mergeInts.AddRange(GetIntRange(range.Start, range.End));
            }

            return mergeInts;
        }
    }
}
