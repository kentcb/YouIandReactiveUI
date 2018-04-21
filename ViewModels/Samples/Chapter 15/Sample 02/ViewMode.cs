namespace Book.ViewModels.Samples.Chapter15.Sample02
{
    using System;

    public enum ViewMode
    {
        Image,
        Details
    }

    public static class ViewModeExtensions
    {
        public static ViewMode Toggle(this ViewMode @this)
        {
            switch (@this)
            {
                case ViewMode.Details:
                    return ViewMode.Image;
                case ViewMode.Image:
                    return ViewMode.Details;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}