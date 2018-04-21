namespace Book.ViewModels.Samples.Chapter04.Sample04
{
    using System;

    // this is our "service" that will be registered in the service locator
    public interface IDinosaurExhibition
    {
        string What
        {
            get;
        }

        DateTimeOffset When
        {
            get;
        }

        string Where
        {
            get;
        }
    }
}