using System.ComponentModel.DataAnnotations;

namespace OtzarSffarrim.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string BookCategory { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int ShelfId { get; set; }
        public Shelf? Shelf { get; set; }

    }
}
