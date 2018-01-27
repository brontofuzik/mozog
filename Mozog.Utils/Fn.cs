using System;
using System.Linq;

namespace Mozog.Utils
{
    public static class Fn
    {
        public static TOut Switch<TIn, TOut>(TIn x, Func<TIn, TOut> @default, params (Predicate<TIn> condition, Func<TIn, TOut> value)[] cases)
        {
            foreach (var @case in cases)
                if (@case.condition(x))
                    return @case.value(x);
            return @default(x);
        }

        public static TOut Switch<TIn, TOut>(TIn x, TOut @default, params (Predicate<TIn> condition, TOut value)[] cases)
            => Switch(x, _ => @default, cases.Select(c => ValueTuple.Create<Predicate<TIn>, Func<TIn, TOut>>(c.condition, _ => c.value)).ToArray());
    }
}
