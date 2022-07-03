using System.Collections;
using System.Collections.Generic;

namespace cinematicket.Models
{
    public class Film
    {
        public string Title { get; set; }
        public List<string> Actors { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public int Release { get; set; }
        public int Duration { get; set; }
        public float Rate { get; set; }
        public string Trailer { get; set; }
    }
}