using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.BL.Repositories.TaskRepo;
using ToDoList.DTOs;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskRepo _taskRepo;
        public TaskController(ITaskRepo taskRepo)
        {
            _taskRepo = taskRepo;
        }
        [HttpGet("/GetAllTasks")]
        public async Task<IActionResult> Get() 
        {
            List<TaskdetailsDto> taskdetailsDtos = await _taskRepo.GetAll();
            if(taskdetailsDtos == null)return NotFound();
            else return Ok(taskdetailsDtos);
        }
        [HttpGet("/GetUserTasks")]
        public async Task<IActionResult> GetUserTasks(String UserId)
        {
            List<TaskdetailsDto> taskdetailsDtos = await _taskRepo.getUserTasks(UserId);
            if (taskdetailsDtos == null) return NotFound();
            else return Ok(taskdetailsDtos);
        }
        [HttpGet("/GetById")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            TaskdetailsDto taskdetailsDto = await _taskRepo.getById(taskId);
            if (taskdetailsDto == null) return NotFound();
            else return Ok(taskdetailsDto);
        }
        [HttpPost("/CreateTask")]
        public async Task<IActionResult> CreateTask(TaskDto taskDto)
        {
            int reslut  = await _taskRepo.Add(taskDto);
            if (reslut == 0) return BadRequest();
            else return Ok();
        }
        [HttpPost("/UpdateTask")]
        public async Task<IActionResult> UpdateTask(int taskId ,TaskUpdateDto taskDto)
        {
            int reslut = await _taskRepo.Edit(taskId , taskDto);
            if (reslut == 0) return BadRequest();
            else return Ok();
        }

        [HttpDelete("/DeleteTask")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            int reslut = await _taskRepo.Delete(taskId);
            if (reslut == 0) return BadRequest();
            else return Ok();
        }
    }
}
