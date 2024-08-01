using System.ComponentModel.DataAnnotations;
namespace OtzarSffarrim.Models

{
    public class Library
    {
        [Key]
        public int LibraryId { get; set; }
        public string LibraryCategory { get; set; }
        public List<Shelf>? Shelves { get; set; }
    }
}
