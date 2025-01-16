using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDpageTest.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }

        // New property to store the uploaded file
        [NotMapped]
        public IFormFile? UploadedFile { get; set; }

        // Property to store the file data as byte array
        public byte[]? FileData { get; set; }

        // Property to store the file name
        public string? FileName { get; set; }

        public Document()
        {
            Title = string.Empty;
            Author = string.Empty;
            CreatedDate = DateTime.Now;
        }
    }
}
