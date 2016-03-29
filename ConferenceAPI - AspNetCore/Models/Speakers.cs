using System;
using System.Collections.Generic;

namespace ConferenceAPI.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }

        public IEnumerable<Session> Sessions { get; set; }
    }

    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Track { get; set; }
        public string SpeakerName { get; set; }   
    }

    public class Registrant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }      
        public string Company { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}