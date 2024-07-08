using System.ComponentModel.DataAnnotations;

namespace ToDoList.DTOs
{
    public class TaskdetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public DateTime Created { get; set; }

        public String FullName { get; set; }
        
        public String UserName { get; set; }
    }
}
