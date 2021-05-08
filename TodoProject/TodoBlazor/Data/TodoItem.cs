namespace TodoBlazor.TodoList {
    public class TodoItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsToggle { get; set; }
        public bool IsDone { get; set; }
    }
}