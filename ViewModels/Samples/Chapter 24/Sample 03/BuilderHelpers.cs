//
// NOTE: this code is ripped straight out of Genesis.TestUtil. I would have referenced it directly, but it also includes a TestScheduler, so it
//       conflicts with that defined by ReactiveUI's test library. Assembly aliases would resolve this, but I thought it better to just include
//       the source directly.
//

namespace Genesis.TestUtil
{
    using System;
    using System.Collections.Generic;

    public interface IBuilder
    {
    }

    public static class BuilderExtensions
    {
        public static TBuilder With<TBuilder, TField>(this TBuilder @this, ref TField field, TField value)
            where TBuilder : IBuilder
        {
            field = value;
            return @this;
        }

        public static TBuilder With<TBuilder, TField>(this TBuilder @this, ref List<TField> field, IEnumerable<TField> values)
            where TBuilder : IBuilder
        {
            if (values == null)
            {
                field = null;
            }
            else
            {
                field.AddRange(values);
            }

            return @this;
        }

        public static TBuilder With<TBuilder, TField>(this TBuilder @this, ref List<TField> field, TField value)
            where TBuilder : IBuilder
        {
            field.Add(value);
            return @this;
        }

        public static TBuilder Without<TBuilder, TField>(this TBuilder @this, ref List<TField> field, Predicate<TField> predicate)
             where TBuilder : IBuilder
        {
            field.RemoveAll(predicate);
            return @this;
        }

        public static TBuilder WithoutAll<TBuilder, TField>(this TBuilder @this, ref List<TField> field)
            where TBuilder : IBuilder
        {
            field.Clear();
            return @this;
        }
    }
}