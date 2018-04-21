namespace Book.ViewModels.Data
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public static class TimelineColors
    {
        public static Color GetColorFor(Era era)
        {
            switch (era)
            {
                case Era.Palaeozoic:
                    return Color.FromArgb(249, 190, 9);
                case Era.Mesozoic:
                    return Color.FromArgb(40, 136, 44);
                case Era.Cenozoic:
                    return Color.FromArgb(12, 69, 135);
                default:
                    throw new NotSupportedException();
            }
        }

        public static Color GetColorFor(Era era, Period period)
        {
            var baseColor = GetColorFor(era);
            var factor = 1f;

            switch (period)
            {
                case Period.Cambrian:
                    factor = 1f;
                    break;
                case Period.Ordovician:
                    factor = 0.9f;
                    break;
                case Period.Silurian:
                    factor = 0.8f;
                    break;
                case Period.Devonian:
                    factor = 0.7f;
                    break;
                case Period.Carboniferous:
                    factor = 0.6f;
                    break;
                case Period.Permian:
                    factor = 0.5f;
                    break;
                case Period.Triassic:
                    factor = 1f;
                    break;
                case Period.Jurassic:
                    factor = 0.9f;
                    break;
                case Period.Cretaceous:
                    factor = 0.8f;
                    break;
                case Period.Paleocene:
                    factor = 1f;
                    break;
                case Period.Eocene:
                    factor = 0.9f;
                    break;
                case Period.Oligocene:
                    factor = 0.8f;
                    break;
                case Period.Miocene:
                    factor = 0.7f;
                    break;
                case Period.Pliocene:
                    factor = 0.6f;
                    break;
                case Period.Pleistocene:
                    factor = 0.5f;
                    break;
                case Period.Holocene:
                    factor = 0.4f;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return Color.FromArgb(
                255,
                (int)(baseColor.R * factor),
                (int)(baseColor.G * factor),
                (int)(baseColor.B * factor));
        }

        public static IEnumerable<(Era era, Period period, Color color)> GetColors()
        {
            var erasWithPeriods = new[]
            {
                (era: Era.Palaeozoic, period: Period.Cambrian),
                (era: Era.Palaeozoic, period: Period.Ordovician),
                (era: Era.Palaeozoic, period: Period.Silurian),
                (era: Era.Palaeozoic, period: Period.Devonian),
                (era: Era.Palaeozoic, period: Period.Carboniferous),
                (era: Era.Palaeozoic, period: Period.Permian),
                (era: Era.Mesozoic, period: Period.Triassic),
                (era: Era.Mesozoic, period: Period.Jurassic),
                (era: Era.Mesozoic, period: Period.Cretaceous),
                (era: Era.Cenozoic, period: Period.Paleocene),
                (era: Era.Cenozoic, period: Period.Eocene),
                (era: Era.Cenozoic, period: Period.Oligocene),
                (era: Era.Cenozoic, period: Period.Miocene),
                (era: Era.Cenozoic, period: Period.Pliocene),
                (era: Era.Cenozoic, period: Period.Pleistocene),
                (era: Era.Cenozoic, period: Period.Holocene)
            };

            foreach (var eraWithPeriod in erasWithPeriods)
            {
                yield return (eraWithPeriod.era, eraWithPeriod.period, GetColorFor(eraWithPeriod.era, eraWithPeriod.period));
            }
        }
    }
}