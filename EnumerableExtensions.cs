using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerableExtensions
{
    public static class EnumerableExtensions
    {
        public static (float Min, float Max) Range(this IEnumerable<float> source)
        {
            var min = float.MaxValue;
            var max = float.MinValue;

            foreach (var value in source)
            {
                if (value < min)
                {
                    min = value;
                }

                if (value > max)
                {
                    max = value;
                }
            }

            return (min, max);
        }

        public static IEnumerable<float> Normalize(this IEnumerable<float> source)
        {
            var (min, max) = source.Range();
            return source.Select(value => (value - min) / (max - min));
        }
    }
}
