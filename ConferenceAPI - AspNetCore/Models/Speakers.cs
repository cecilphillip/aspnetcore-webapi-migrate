using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConferenceAPI.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }

        public IEnumerable<Session> Sessions { get; set; }
    }

    public class Session
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Track { get; set; }
        public string SpeakerName { get; set; }   
    }

    public class Registrant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }      
        public string Company { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}