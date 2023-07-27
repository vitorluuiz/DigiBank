using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Utils
{
    public class MockData
    {
        public static class Logo
        {
            public static int Count()
            {
                List<int> list = NumberMagik.MergeIntRanges(new List<NumberRange>
                {
                    new NumberRange
                    {
                        Start = 223,
                        End = 226
                    }, new NumberRange
                    {
                        Start = 229,
                        End = 233,
                    }, new NumberRange
                    {
                        Start = 245,
                        End = 249
                    }, new NumberRange
                    {
                        Start = 266,
                        End = 283
                    }, new NumberRange
                    {
                        Start = 290,
                        End = 296
                    }, new NumberRange
                    {
                        Start = 298,
                        End = 299
                    }
                });

                return (int)list.Count;
            }

            public static int Get(int skips)
            {
                List<int> list = NumberMagik.MergeIntRanges(new List<NumberRange>
                {
                    new NumberRange
                    {
                        Start = 223,
                        End = 226
                    }, new NumberRange
                    {
                        Start = 229,
                        End = 233,
                    }, new NumberRange
                    {
                        Start = 245,
                        End = 249
                    }, new NumberRange
                    {
                        Start = 266,
                        End = 283
                    }, new NumberRange
                    {
                        Start = 290,
                        End = 296
                    }, new NumberRange
                    {
                        Start = 298,
                        End = 299
                    }
                });

                return list.Skip(skips).First();
            }
        }
    }
}
