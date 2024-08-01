using Microsoft.AspNetCore.Mvc.Rendering;

namespace OtzarSffarrim.Models
{
    public class BookView
    {
        public Book Book { get; set; }
        //public List<Library> Libraries { get; set; }
        public List<string> Categories { get; set; }

    }
}
