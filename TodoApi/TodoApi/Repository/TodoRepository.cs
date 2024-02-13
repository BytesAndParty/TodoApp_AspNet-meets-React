using System.Text.Json;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public class TodoRepository
    {
        private const string FilePath = "Data/todoitems.json";

        public static List<TodoItem> LoadTodoItems()
        {
            if (File.Exists(FilePath))
            {
                var jsonString = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<TodoItem>>(jsonString) ?? new List<TodoItem>();
            }

            return new List<TodoItem>();
        }

        public static void SaveTodoItems(List<TodoItem> items)
        {
            var jsonString = JsonSerializer.Serialize(items);
            File.WriteAllText(FilePath, jsonString);
        }
    }
}
