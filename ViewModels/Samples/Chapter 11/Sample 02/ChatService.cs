namespace Book.ViewModels.Samples.Chapter11.Sample02
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Subjects;

    public sealed class ChatService : IChatService
    {
        public static readonly ChatService Instance = new ChatService();
        private readonly IDictionary<string, ISubject<ChatMessage>> roomSubjects;

        public ChatService()
        {
            this.roomSubjects = new Dictionary<string, ISubject<ChatMessage>>();
        }

        public IDisposable Publish(string room, IObservable<ChatMessage> messages) =>
            messages.Subscribe(GetSubjectForRoom(room));

        public IObservable<ChatMessage> Listen(string room) =>
            this.GetSubjectForRoom(room);

        private ISubject<ChatMessage> GetSubjectForRoom(string room)
        {
            ISubject<ChatMessage> subject;

            if (!this.roomSubjects.TryGetValue(room, out subject))
            {
                subject = new Subject<ChatMessage>();
                this.roomSubjects[room] = subject;
            }

            return subject;
        }
    }
}