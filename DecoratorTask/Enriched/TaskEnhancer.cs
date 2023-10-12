using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Enriched;

public abstract class TaskEnhancer : ITask
{
    public ITask Task { get; private set; }
    public int Id { get => Task.Id; }
    public string Title { get => Task.Title; set => Task.Title = value; }
    public string Description { get => Task.Description; set => Task.Description = value; }
    public State StateTask { get => Task.StateTask; set => Task.StateTask = value; }

    public TaskEnhancer(ITask task) => Task = task ;

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public abstract string Info();

    public abstract void CompleteTask();
}
