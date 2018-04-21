namespace Book.ViewModels.Samples.Chapter11.Sample02
{
    using System;

    public interface IChatService
    {
        IObservable<ChatMessage> Listen(string room);

        IDisposable Publish(string room, IObservable<ChatMessage> messages);
    }
}