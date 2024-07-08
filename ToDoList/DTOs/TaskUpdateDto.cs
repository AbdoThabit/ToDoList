namespace ToDoList.DTOs
{
    public class TaskUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public DateTime Created { get; set; }
        
    }
}
