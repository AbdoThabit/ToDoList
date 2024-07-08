using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.DAL.Entites
{
    public class JobTask
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        [Required]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(150)")]
        [Required]
        public string Description { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        [Required]
        public DateTime date { get; set; }
        public string Type { get; set; }
        [ForeignKey("user")]
        public string UserId { get; set; }
        public virtual ApplicationUser user { get; set; }

    }
}
