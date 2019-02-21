using Microsoft.AspNetCore.Mvc;

namespace Confetti.Api.Controllers
{
    public class ToDoItem
    {
        public string Name { get; set; }

        public ToDoItem(string name)
        {
            Name = name;
        }
    }

    public class ToDoController : Controller
    {
        [HttpGet]
        [Route("/api/todo")]
        public ToDoItem[] GetAllToDoItems()
        {
            return new []
            {
                new ToDoItem("todo1"),
                new ToDoItem("todo2"),
                new ToDoItem("todo3")
            };
        }
    }
}