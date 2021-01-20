using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;

namespace EnumerableExtensions
{
    class Tests
    {
        static void Main()
        {
            Prop.ForAll(new FiniteFloats(), values =>
            {
                var (min, max) = values.Range();
                return MaxIsGreatestValue(values, max).And(MinIsLowestValue(values, min))
                    .When(values.Length > 0);
            }).QuickCheck("Range");

            Prop.ForAll(new FiniteFloats(), values =>
            {
                var normalized = values.Normalize();
                return BetweenZeroAndOne(normalized)
                    .Trivial(values.Length == 0);
            }).QuickCheck();

            if (!BetweenZeroAndOne(new float[] { float.MaxValue - 1, float.MinValue + 1 }.Normalize()))
            {
                throw new Exception("Oh no :(");
            }
        }

        static bool MaxIsGreatestValue(IEnumerable<float> values, float max)
        {
            return values.All(value => value <= max) && values.Any(value => value == max);
        }

        static bool MinIsLowestValue(IEnumerable<float> values, float min)
        {
            return values.All(value => value >= min) && values.Any(value => value == min);
        }

        static bool BetweenZeroAndOne(IEnumerable<float> values)
        {
            return values.All(value => value >= 0 && value <= 1);
        }
    }

    class FiniteFloats : Arbitrary<float[]>
    {
        public override Gen<float[]> Generator
        {
            get
            {
                return Gen.ListOf(Arb.Generate<float>())
                    .Where(values => values.All(value => float.IsFinite(value) && value < float.MaxValue && value > float.MinValue))
                    .Select(values => values.ToArray());
            }
        }

        public override IEnumerable<float[]> Shrinker(float[] values)
        {
            var shrunk = new List<float[]>();

            for (var i = 0; i < values.Length; i++)
            {
                shrunk.Add(values.Where((_, index) => index != i).ToArray());
            }

            return shrunk;
        }
    }
}
