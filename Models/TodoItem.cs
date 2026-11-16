using System.ComponentModel.DataAnnotations;

namespace TodoApiDotNet.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}