using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Entities;

public class BasicTask : ITask
{
    private int IdTask;
    public int Id { get => IdTask; set { if (IdTask == 0) IdTask = value; } }
    public string Title { get; set; }
    public string Description { get; set; }
    public State StateTask { get; set; }

    public BasicTask(int id = -1)
    {
        Title = "New Task";
        Description = string.Empty;
        Id = id == -1 ? GetHashCode() : id;
        StateTask = State.Expectation;
    }

    public BasicTask(string title, string description, State stateTask = State.Expectation, int id = -1)
    {
        Title = title;
        Description = description;
        Id = id == -1 ? GetHashCode() : id;
        StateTask = stateTask;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public string Info()
        => $"Id: {Id}, Titel: {Title}, Description: {Description}, State: {StateTask} | ";

    public void CompleteTask()
        => StateTask = State.Complete;
}
