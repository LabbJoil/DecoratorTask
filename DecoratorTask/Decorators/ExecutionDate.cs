using DecoratorTask.Enriched;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace DecoratorTask.Decorators
{
    public class ExecutionDate : TaskEnhancer
    { 
        public DateTime DateStartTask { get; private set; }
        public DateTime DateEndTask { get; private set; }
        public Repeat OftenRepeat { get; private set; }

        public ExecutionDate(ref ITask task) : base(task)
        {
            DateTime nowDateTime = DateTime.Now;
            DateStartTask = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, 0).AddHours(1);
            DateEndTask = DateStartTask.AddHours(1);

            OftenRepeat = Repeat.None;
            StateTask = State.Expectation;
            ChangeTask(task, this);
            task = null;
        }

        public ExecutionDate(ref ITask? task, Repeat oftenRepeat, DateTime dateStartTask, DateTime dateEndTask) : base(task)
        {
            DateTime nowDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            if (dateStartTask < nowDate || dateEndTask < nowDate)
                throw new Exception("Даты должны быть больше или равны сегодняшней дате");
            else if (dateStartTask > dateEndTask)
                throw new Exception("Дата начала должна быть меньше даты окончания");

            CheckCorrectStatusRepeat(oftenRepeat, dateStartTask, dateEndTask);
            DateStartTask = new DateTime(dateStartTask.Year, dateStartTask.Month, dateStartTask.Day, dateStartTask.Hour, dateStartTask.Minute, 0);
            DateEndTask = new DateTime(dateEndTask.Year, dateEndTask.Month, dateEndTask.Day, dateEndTask.Hour, dateEndTask.Minute, 0);
            OftenRepeat = oftenRepeat;
            StateTask = State.Expectation;
            ChangeTask(task, this);
            task = null;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        private static void CheckCorrectStatusRepeat(Repeat repeat, DateTime dateStart, DateTime dateEnd)
        {
            switch (repeat)
            {
                case Repeat.Everyday:
                    if ((dateEnd - dateStart).Days > 1)
                        throw new Exception("Продолжительность задачи должна быть меньше или равна дню");
                    break;

                case Repeat.EveryWeek:
                    if ((dateEnd - dateStart).Days > 7)
                        throw new Exception("Продолжительность задачи должна быть меньше или равна недели");
                    break;

                case Repeat.EveryMonth:
                    if ((dateEnd - dateStart).Days > DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                        throw new Exception("Продолжительность задачи должна быть меньше или равна месяцу");
                    break;

                case Repeat.EveryYear:
                    if ((dateEnd - dateStart).Days > DateTime.Now.DayOfYear)
                        throw new Exception("Продолжительность задачи должна быть меньше или равна месяцу");
                    break;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        public void ChangeDateStart(DateTime newDateStart)
        {
            DateTime nowDate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            if (newDateStart < nowDate || DateEndTask < newDateStart)
                throw new Exception("Неправильно указана дата");
            CheckCorrectStatusRepeat(OftenRepeat, newDateStart, DateEndTask);
            StateTask = State.Expectation;
            DateStartTask = new DateTime(newDateStart.Year, newDateStart.Month, newDateStart.Day, newDateStart.Hour, newDateStart.Minute, 0);
        }

        public void ChangeDateEnd(DateTime newDateEnd)
        {
            if (newDateEnd < DateTime.Now || newDateEnd < DateStartTask) 
                throw new Exception("Неправильно указана дата");
            CheckCorrectStatusRepeat(OftenRepeat, newDateEnd, DateEndTask);
            DateEndTask = new DateTime(newDateEnd.Year, newDateEnd.Month, newDateEnd.Day, newDateEnd.Hour, newDateEnd.Minute, 0);
        }

        public void ChangeRepeat(Repeat oftenRepeat)
        {
            CheckCorrectStatusRepeat(oftenRepeat, DateStartTask, DateEndTask);
            OftenRepeat = oftenRepeat;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        public void Checked()
        {
            DateTime dateNow = DateTime.Now;

            if (StateTask == State.Overdue)
            {
                if (dateNow < DateEndTask)
                    DateEndTask = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, dateNow.Minute, 0);
                return;
            }
                
            bool isNowTime = dateNow.TimeOfDay >= DateStartTask.TimeOfDay && dateNow.TimeOfDay <= DateEndTask.TimeOfDay;

            switch (OftenRepeat)
            {
                case Repeat.Everyday:
                    if (isNowTime)
                        StateTask = State.InProcess;
                    else StateTask = State.Expectation;
                    break;

                case Repeat.EveryWeek:
                    if (isNowTime && (dateNow.DayOfWeek >= DateStartTask.DayOfWeek || dateNow.DayOfWeek <= DateEndTask.DayOfWeek))
                        StateTask = State.InProcess;
                    else StateTask = State.Expectation;
                    break;

                case Repeat.EveryMonth:
                    if (isNowTime && (dateNow.Day >= DateStartTask.Day || dateNow.Day <= DateEndTask.Day))
                        StateTask = State.InProcess;
                    else StateTask = State.Expectation;
                    break;

                case Repeat.EveryYear:
                    if (isNowTime && (dateNow.DayOfYear >= DateStartTask.DayOfYear || dateNow.DayOfYear <= DateEndTask.DayOfYear))
                        StateTask = State.InProcess;
                    else StateTask = State.Expectation;
                    break;

                case Repeat.None:
                    if (dateNow >= DateStartTask && dateNow < DateEndTask)
                        StateTask = State.InProcess;
                    else if (dateNow < DateStartTask) StateTask = State.Expectation;
                    else StateTask = State.Overdue;
                    break;
            }
            
        }

        public override string Info()
            => $"TimeStart: {DateStartTask}, TimeEnd: {DateEndTask}, OftenRepeat: {OftenRepeat} | " + Task.Info();
    }
}
