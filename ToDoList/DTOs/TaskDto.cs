namespace ToDoList.DTOs
{
    public class TaskDto
    {
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public DateTime Created { get; set; }
        public String UserID { get; set; }
    }
}
