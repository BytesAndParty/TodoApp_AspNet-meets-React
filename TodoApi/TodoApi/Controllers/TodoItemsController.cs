using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private List<TodoItem> _todoItems;
        private static long _nextId = 1;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ILogger<TodoItemsController> logger)
        {
            _logger = logger;
            _todoItems = TodoRepository.LoadTodoItems();
        }

        // GET: api/TodoItems
        [HttpGet]
        public IEnumerable<TodoItem> GetTodoItems()
        {
            _logger.LogInformation("Getting all todo items.");
            return _todoItems;
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public ActionResult<TodoItem> GetTodoItem(long id)
        {
            _logger.LogInformation($"Getting todo item with id {id}.");
            var todoItem = _todoItems.FirstOrDefault(t => t.Id == id);

            if (todoItem == null)
            {
                _logger.LogWarning($"Todo item with id {id} not found.");
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/TodoItems
        [HttpPost]
        public ActionResult<TodoItem> PostTodoItem(TodoItem item)
        {
            _logger.LogInformation($"Adding new todo item with name {item.Name}.");
            item.Id = _nextId++;
            _todoItems.Add(item);
            TodoRepository.SaveTodoItems(_todoItems);
            _logger.LogInformation($"Todo item with id {item.Id} added successfully.");
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public IActionResult PutTodoItem(long id, TodoItem item)
        {
            _logger.LogInformation($"Updating todo item with id {id}.");
            var index = _todoItems.FindIndex(t => t.Id == id);
            if (index == -1)
            {
                _logger.LogWarning($"Todo item with id {id} not found.");
                return NotFound();
            }

            _todoItems[index] = item;
            TodoRepository.SaveTodoItems(_todoItems);
            _logger.LogInformation($"Todo item with id {id} updated successfully.");
            return NoContent();
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTodoItem(long id)
        {
            _logger.LogInformation($"Deleting todo item with id {id}.");
            var index = _todoItems.FindIndex(t => t.Id == id);
            if (index == -1)
            {
                _logger.LogWarning($"Todo item with id {id} not found.");
                return NotFound();
            }

            _todoItems.RemoveAt(index);
            TodoRepository.SaveTodoItems(_todoItems);
            _logger.LogInformation($"Todo item with id {id} deleted successfully.");
            return NoContent();
        }
    }
}
