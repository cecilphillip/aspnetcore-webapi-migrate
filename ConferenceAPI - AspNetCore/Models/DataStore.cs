using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bogus;

namespace ConferenceAPI.Models
{
    public class DataStore : IDataStore
    {
        private List<Registrant> Registrants { get; set; }
        private List<Speaker> Speakers { get; set; }
        private const int PAGE_SIZE = 10;

        public DataStore()
        {
            LoadFakeData();
        }

        private void LoadFakeData()
        {
            var regId = 0;
            var start = DateTime.Parse("6/15/1980", CultureInfo.InvariantCulture);
            var end = DateTime.Parse("8/15/1992", CultureInfo.InvariantCulture);

            Registrants = new Faker<Registrant>()
                .RuleFor(r => r.Id, f => regId++)
                .RuleFor(r => r.Name, f => f.Name.FirstName() + " " + f.Name.LastName())
                .RuleFor(r => r.DateOfBirth, f => f.Date.Between(start, end))
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .RuleFor(r => r.Company, f => f.Company.CompanyName())
                .Generate(10).ToList();

            var sessionId = 0;
            string[] tracks = { "Agile", "Mobile", "Cloud", "HTML5", "IoT", "JavaScript", "Open Source", "NoSQL", "Performance" };
            string[] title = { "For Business People", " FTW!", "The ONETUG Edition",
                               "and More", " like you've never seen it", "in 40 minutes",
                               "for the kast time, I promise", "the cool parts", "and Friends!",
                                "Tips & Tricks"};
            var fakeSessions = new Faker<Session>()
                .RuleFor(r => r.Id, f => sessionId++)                
                .RuleFor(r => r.Track, f => f.PickRandom(tracks))
                .FinishWith((faker, session) =>
                {
                    session.Name = $"{session.Track} {faker.PickRandom(title)}";
                });


            var speakerId = 0;
            var random = new Random();
            Speakers = new Faker<Speaker>()
                .RuleFor(r => r.Id, f => speakerId++)
                .RuleFor(r => r.Name, f => f.Name.FirstName() + " " + f.Name.LastName())
                .RuleFor(r => r.Company, f => f.Company.CompanyName())
                .RuleFor(r => r.Bio, f => f.Lorem.Paragraph())
                .RuleFor(r => r.Sessions, f => fakeSessions.Generate(random.Next(1, 3)).ToList())
                .FinishWith((faker, speaker) =>
                {
                    speaker.Sessions.ForEach(s => s.SpeakerName = speaker.Name);
                })
                .Generate(20).ToList();
        }

        public IEnumerable<Speaker> GetSpeakers()
        {
            return Speakers;
        }

        public IEnumerable<Speaker> GetSpeakers(int page)
        {
            return Speakers.Skip(PAGE_SIZE * (page - 1)).Take(PAGE_SIZE);
        }

        public OperationResult<Speaker> AddSpeaker(Speaker speaker)
        {
            speaker.Id = Speakers.Last().Id + 1;
            Speakers.Add(speaker);
            return new OperationResult<Speaker>(OperationStatus.Success, "Added", speaker);
        }

        public OperationResult<Speaker> RemoveSpeaker(int id)
        {
            var speaker = Speakers.SingleOrDefault(s => s.Id == id);
            if (speaker != null)
            {
                Speakers.Remove(speaker);
                return new OperationResult<Speaker>(OperationStatus.Success, $"{speaker.Name} removed", speaker);
            }
            return new OperationResult<Speaker>(OperationStatus.Failure, "No associated speaker found", null);
        }

        public IEnumerable<Registrant> GetRegistrants()
        {
            return Registrants;
        }

        public IEnumerable<Session> GetSessions()
        {
            return Speakers.SelectMany(sp => sp.Sessions);
        }
    }

    public interface IDataStore
    {
        IEnumerable<Speaker> GetSpeakers();
        IEnumerable<Speaker> GetSpeakers(int value);
        OperationResult<Speaker> AddSpeaker(Speaker speaker);
        OperationResult<Speaker> RemoveSpeaker(int id);
        IEnumerable<Registrant> GetRegistrants();
        IEnumerable<Session> GetSessions();
    }
}