using DecoratorTask.Enriched;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTask.Decorators;

public class ExecutionDate : TaskEnhancer
{ 
    public DateTime DateStartTask { get; private set; }
    public DateTime DateEndTask { get; private set; }
    public Repeat OftenRepeat { get; private set; }

    public ExecutionDate(ITask task) : base(task)
    {
        if (CustomTask.GetExecutionDateTask(task) != null) throw new Exception("ExecutionDateTask уже используется как один из декорирующих элементов.");
        DateTime nowDateTime = DateTime.Now;
        DateStartTask = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, 0).AddHours(1);
        DateEndTask = DateStartTask.AddHours(1);

        OftenRepeat = Repeat.None;
        Checked();
    }

    public ExecutionDate(ITask task, Repeat oftenRepeat, DateTime dateStartTask, DateTime dateEndTask) : base(task)
    {
        if (CustomTask.GetExecutionDateTask(task) != null) throw new Exception("ExecutionDateTask уже используется как один из декорирующих элементов.");
        if (dateStartTask > dateEndTask)
            throw new Exception("Дата окончания должна быть больше даты начала");

        CheckCorrectStatusRepeat(oftenRepeat, dateStartTask, dateEndTask);
        DateStartTask = new DateTime(dateStartTask.Year, dateStartTask.Month, dateStartTask.Day, dateStartTask.Hour, dateStartTask.Minute, 0);
        DateEndTask = new DateTime(dateEndTask.Year, dateEndTask.Month, dateEndTask.Day, dateEndTask.Hour, dateEndTask.Minute, 0);
        OftenRepeat = oftenRepeat;
        Checked();
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public void ChangeDateStart(DateTime newDateStart)
    {
        if (newDateStart >= DateEndTask)
            throw new Exception("Дата начала должна быть меньше даты окончания");
        CheckCorrectStatusRepeat(OftenRepeat, newDateStart, DateEndTask);
        DateStartTask = new DateTime(newDateStart.Year, newDateStart.Month, newDateStart.Day, newDateStart.Hour, newDateStart.Minute, 0);
        Checked();
    }

    public void ChangeDateEnd(DateTime newDateEnd)
    {
        if (DateStartTask > newDateEnd) 
            throw new Exception("Дата окончания должна быть больше даты начала и сегодняшней даты");
        CheckCorrectStatusRepeat(OftenRepeat, newDateEnd, DateEndTask);
        DateEndTask = new DateTime(newDateEnd.Year, newDateEnd.Month, newDateEnd.Day, newDateEnd.Hour, newDateEnd.Minute, 0);
    }

    public void ChangeRepeat(Repeat oftenRepeat)
    {
        CheckCorrectStatusRepeat(oftenRepeat, DateStartTask, DateEndTask);
        OftenRepeat = oftenRepeat;
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    public override void CompleteTask()
        => StateTask = CalculateNextTaskDates();

    public void Checked()
    {
        DateTime dateNow = DateTime.Now;
        if (dateNow >= DateStartTask && dateNow < DateEndTask)
            StateTask = State.InProcess;
        else if (dateNow < DateStartTask)
            StateTask = State.Expectation;
        else if (dateNow >= DateEndTask)
            StateTask = CalculateNextTaskDates();
    }

    public override string Info()
        => $"TimeStart: {DateStartTask}, TimeEnd: {DateEndTask}, OftenRepeat: {OftenRepeat} | " + Task.Info();

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    private static void CheckCorrectStatusRepeat(Repeat repeat, DateTime dateStart, DateTime dateEnd)
    {
        switch (repeat)
        {
            case Repeat.EveryDay:
                if ((dateEnd - dateStart).TotalDays > 1)
                    throw new Exception("Продолжительность задачи должна быть меньше или равна дню");
                break;

            case Repeat.EveryWeek:
                if ((dateEnd - dateStart).TotalDays > 7)
                    throw new Exception("Продолжительность задачи должна быть меньше или равна недели");
                break;

            case Repeat.EveryMonth:
                if ((dateEnd - dateStart).TotalDays > DateTime.DaysInMonth(dateStart.Year, dateStart.Month))
                    throw new Exception("Продолжительность задачи должна быть меньше или равна месяцу");
                break;

            case Repeat.EveryYear:
                if ((dateEnd - dateStart).TotalDays > 365)
                    throw new Exception("Продолжительность задачи должна быть меньше или равна году");
                break;
        }
    }

    private State CalculateNextTaskDates()
    {
        State stateTask = State.Expectation;
        DateTime dateNow = DateTime.Now;
        DateTime nextDateStartTask, nextDateEndTask;
        DateTime baseDate = dateNow > DateStartTask ? dateNow : DateStartTask;

        switch (OftenRepeat)
        {
            case Repeat.EveryDay:
                int addDay = (baseDate.TimeOfDay > DateStartTask.TimeOfDay) ? 0 : 1;
                nextDateStartTask = baseDate.AddDays(addDay);
                nextDateEndTask = baseDate.AddDays(addDay);

                DateStartTask = new DateTime(nextDateStartTask.Year, nextDateStartTask.Month, nextDateStartTask.Day, DateStartTask.Hour, DateStartTask.Minute, 0);
                DateEndTask = new DateTime(nextDateEndTask.Year, nextDateEndTask.Month, nextDateEndTask.Day, DateEndTask.Hour, DateEndTask.Minute, 0);
                break;

            case Repeat.EveryWeek:
                int mainDay = (baseDate.DayOfWeek < DateStartTask.DayOfWeek) ? 0 : 7;
                nextDateStartTask = baseDate.AddDays(mainDay - (int)baseDate.DayOfWeek + (int)DateStartTask.DayOfWeek);
                nextDateEndTask = nextDateStartTask.AddDays((DateEndTask - DateStartTask).Days);

                DateStartTask = new DateTime(nextDateStartTask.Year, nextDateStartTask.Month, nextDateStartTask.Day, DateStartTask.Hour, DateStartTask.Minute, 0);
                DateEndTask = new DateTime(nextDateEndTask.Year, nextDateEndTask.Month, nextDateEndTask.Day, DateEndTask.Hour, DateEndTask.Minute, 0);
                break;

            case Repeat.EveryMonth:
                int addMonth = (baseDate.Day < DateStartTask.Day) ? 0 : 1;
                nextDateStartTask = new DateTime(baseDate.Year, baseDate.AddMonths(addMonth).Month, 1);
                nextDateStartTask = nextDateStartTask.AddDays(DateStartTask.Day - 1);
                nextDateEndTask = nextDateStartTask.AddDays((DateEndTask - DateStartTask).Days);

                DateStartTask = new DateTime(nextDateStartTask.Year, nextDateStartTask.Month, nextDateStartTask.Day, DateStartTask.Hour, DateStartTask.Minute, 0);
                DateEndTask = new DateTime(nextDateEndTask.Year, nextDateEndTask.Month, nextDateEndTask.Day, DateEndTask.Hour, DateEndTask.Minute, 0);
                break;

            case Repeat.EveryYear:
                int addYear = (baseDate.DayOfYear < DateStartTask.DayOfYear) ? 0 : 1;
                nextDateStartTask = new DateTime(baseDate.AddYears(addYear).Year, DateStartTask.Month, 1);
                nextDateStartTask = nextDateStartTask.AddDays(DateStartTask.Day - 1);
                nextDateEndTask = nextDateStartTask.AddDays((DateEndTask - DateStartTask).Days);

                DateStartTask = new DateTime(nextDateStartTask.Year, nextDateStartTask.Month, nextDateStartTask.Day, DateStartTask.Hour, DateStartTask.Minute, 0);
                DateEndTask = new DateTime(nextDateEndTask.Year, nextDateEndTask.Month, nextDateEndTask.Day, DateEndTask.Hour, DateEndTask.Minute, 0);
                break;

            case Repeat.None:
                if (DateStartTask > dateNow) DateStartTask = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, dateNow.Minute, 0);
                if (DateEndTask > dateNow) DateEndTask = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, dateNow.Minute, 0);
                stateTask = State.Complete;
                break;
        }
        return stateTask;
    }
}
