using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Entities;

public class BasicTask : ITask
{
    private readonly int Id;
    public string Title { get; set; }
    public string Description { get; set; }
    public State StateTask { get; private set; }
    public static List<ITask> AllTask = new();

    public BasicTask()
    {
        Title = "New Task";
        Description = string.Empty;
        Id = GetHashCode();
        StateTask = State.Expectation;
        AllTask.Add(this);
    }

    public BasicTask(string name, string description, State state = State.Expectation)
    {
        Title = name;
        Description = description;
        Id = GetHashCode();
        StateTask = state;
        AllTask.Add(this);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public int GetId() => Id;
    public State GetState() => StateTask;

    public string Info()
        => $"Id: {Id}, Titel: {Title}, Description: {Description}, State: {StateTask} | ";

    public void DeleateTask(ref ITask deleteTask)
    {
        if (!AllTask.Remove(deleteTask))
            throw new Exception("Объекта не существует");
        deleteTask = null!;
    }

    public void ChangeTask(ITask lastTask, ITask newTask)
    {
        if (!AllTask.Remove(lastTask)) 
            throw new Exception("Объекта не существует или уже используется в другой задаче.");
        AllTask.Add(newTask);
    }

    public void CompleteTask()
        => ChangeState(State.Complete);

    public void ChangeState(State newState)
        => StateTask = newState;
}
