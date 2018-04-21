namespace Book.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using CommonMark;
    using CommonMark.Syntax;
    using ReactiveUI;

    [DebuggerDisplay("Sample {Number}: {Name}")]
    public sealed class SampleDetailsViewModel : ReactiveObject
    {
        private readonly string name;
        private readonly int number;
        private readonly string description;
        private readonly TypeInfo mainViewModelType;
        private IReactiveObject currentMainViewModel;

        public SampleDetailsViewModel(
            string name,
            int number,
            string description,
            TypeInfo mainViewModelType)
        {
            this.name = name;
            this.number = number;
            this.description = description;
            this.mainViewModelType = mainViewModelType;
        }

        public string Name => this.name;

        public int Number => this.number;

        public string NumberDisplay => Number.ToString("00");

        public string Description => this.description;

        public SampleType SampleType
        {
            get
            {
#if NETFX
                if (IsSubclassOfRawGeneric(typeof(PerformanceSampleViewModel<>), this.mainViewModelType))
                {
                    return SampleType.Performance;
                }
#endif

                if (typeof(ConsoleSampleViewModel).GetTypeInfo().IsAssignableFrom(this.mainViewModelType))
                {
                    return SampleType.Console;
                }

                if (typeof(TestSampleViewModel).GetTypeInfo().IsAssignableFrom(this.mainViewModelType))
                {
                    return SampleType.Test;
                }

                return SampleType.UI;
            }
        }

        public IReactiveObject GetMainViewModel(bool createNew = true)
        {
            if (createNew)
            {
                this.currentMainViewModel = (IReactiveObject)Activator.CreateInstance(this.mainViewModelType.AsType());
            }

            return this.currentMainViewModel;
        }

        public Block SampleDescriptionDocument =>
            CommonMarkConverter
                .Parse(this.description);

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.GetTypeInfo().IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;

                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.GetTypeInfo().BaseType;
            }

            return false;
        }
    }
}