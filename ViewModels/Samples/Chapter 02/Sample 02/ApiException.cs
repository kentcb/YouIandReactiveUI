namespace Book.ViewModels.Samples.Chapter02.Sample02
{
    using System;

    public sealed class ApiException : Exception
    {
        public ApiException()
            : base("It seems you've not supplied Twitter API keys. You'll need to head to https://apps.twitter.com/ to set up an app, then copy and paste the details into the source code.")
        {
        }
    }
}