﻿using System;
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
        }

        static bool MaxIsGreatestValue(IEnumerable<float> values, float max)
        {
            return values.All(value => value <= max) && values.Any(value => value == max);
        }

        static bool MinIsLowestValue(IEnumerable<float> values, float min)
        {
            return values.All(value => value >= min) && values.Any(value => value == min);
        }
    }

    class FiniteFloats : Arbitrary<float[]>
    {
        public override Gen<float[]> Generator
        {
            get
            {
                return Gen.ListOf(Arb.Generate<float>())
                    .Where(values => values.All(value => float.IsFinite(value)))
                    .Select(values => values.ToArray());
            }
        }
    }
}
