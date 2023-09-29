using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Enriched
{
    public abstract class TaskEnhancer : ITask
    {
        public ITask Task;
        public string Title { get => Task.Title; set => Task.Title = value; }
        public string Description { get => Task.Description; set => Task.Description = value; }
        public State StateTask { get => Task.StateTask; set => Task.StateTask = value; }

        public TaskEnhancer(ITask? task) => Task = task ?? throw new Exception("Ссылка на задачу равна null");

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        public int GetId() => Task.GetId();
        public abstract string Info();

        public void DeleateTask(ref ITask deleteTask)
            => Task.DeleateTask(ref deleteTask);

        public void ChangeTask(ITask lastTask, ITask newTask)
            => Task.ChangeTask(lastTask, newTask);

    }
}
