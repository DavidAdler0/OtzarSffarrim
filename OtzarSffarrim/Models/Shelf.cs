using System.ComponentModel.DataAnnotations;

namespace OtzarSffarrim.Models
{
    public class Shelf
    {
        [Key]
        public int ShelfId { get; set; }
        public int Width { get; set; }
        public int? FreeWidth { get; set; }
        public int Height { get; set; }
        public List<Book>? Books { get; set; }

        public int LibraryId { get; set; }
        public Library? Library { get; set; }


    }
}
