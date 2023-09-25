using DecoratorTask.Decorators;
using DecoratorTask.Entities;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorTaskTest
{
    [TestClass]
    public class ExecutionDateTest
    {
        private readonly ITask TestTask = new BasicTask();
        private readonly DateTime StartDate = DateTime.ParseExact(string.Join(".", new string[] { "10", "10", "2050", "14", "30" }), "d.M.yyyy.HH.mm", null);
        private readonly DateTime EndDate = DateTime.ParseExact(string.Join(".", new string[] { "16", "10", "2050", "15", "30" }), "d.M.yyyy.HH.mm", null);

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Constructor_DefaultValues()
        {
            // Arrange
            DateTime nowDateTime = DateTime.Now;
            DateTime dateStartTask = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, 0).AddHours(1);
            DateTime dateEndTask = dateStartTask.AddHours(1);

            // Act
            ExecutionDate executionDate = new(TestTask);

            // Assert
            Assert.AreEqual(dateStartTask.Hour, executionDate.DateStartTask.Hour);
            Assert.AreEqual(dateEndTask.Hour, executionDate.DateEndTask.Hour);
            Assert.AreEqual(Repeat.None, executionDate.OftenRepeat);
        }

        [TestMethod]
        public void Constructor_Full()
        {
            // Arrange
            Repeat repeat = Repeat.EveryWeek;
            DateTime startDate = DateTime.ParseExact(string.Join(".", new string[]{ "10", "10", "2050", "14", "30" }), "d.M.yyyy.HH.mm", null);
            DateTime endDate = DateTime.ParseExact(string.Join(".", new string[] { "16", "10", "2050", "15", "30" }), "d.M.yyyy.HH.mm", null);

            // Act
            ExecutionDate executionDate = new(TestTask, repeat, startDate, endDate);

            // Assert
            Assert.AreEqual(DateTime.ParseExact("10.10.2050 14:30", "d.M.yyyy HH:mm", null), executionDate.DateStartTask);
            Assert.AreEqual(DateTime.ParseExact("16.10.2050 15:30", "d.M.yyyy HH:mm", null), executionDate.DateEndTask);
            Assert.AreEqual(Repeat.EveryWeek, executionDate.OftenRepeat);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Constructor_InValidRepeatInput_ThrowsException()
        {
            // Arrange
            Repeat repeat = Repeat.Everyday;

            // Act
            Assert.ThrowsException<Exception>(() 
                => new ExecutionDate(TestTask, repeat, StartDate, EndDate));
        }

        [TestMethod]
        public void Constructor_InValidStartDateInput_ThrowsException()
        {
            // Arrange
            Repeat repeat = Repeat.None;
            DateTime IncorrectStartDate = DateTime.ParseExact(string.Join(".", new string[] { "17", "10", "2050", "15", "30" }), "d.M.yyyy.HH.mm", null);

            // Act
            Assert.ThrowsException<Exception>(() => new ExecutionDate(TestTask, repeat, IncorrectStartDate, EndDate));
        }

        [TestMethod]
        public void Constructor_InValidEndDateInput_ThrowsException()
        {
            // Arrange
            Repeat repeat = Repeat.None;
            DateTime IncorrectEndDate = DateTime.ParseExact(string.Join(".", new string[] { "09", "10", "2050", "15", "30" }), "d.M.yyyy.HH.mm", null);

            // Act
            Assert.ThrowsException<Exception>(() => new ExecutionDate(TestTask, repeat, StartDate, IncorrectEndDate));
        }

        [TestMethod]
        public void Constructor_InValidDateInput_ThrowsException()
        {
            // Arrange
            Repeat repeat = Repeat.None;
            DateTime IncorrectEndDate = DateTime.ParseExact(string.Join(".", new string[] { "09", "09", "2012", "15", "30" }), "d.M.yyyy.HH.mm", null);

            // Act
            Assert.ThrowsException<Exception>(() => new ExecutionDate(TestTask, repeat, StartDate, IncorrectEndDate));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ChangeDateStart_ValidInput()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask, Repeat.None, StartDate, EndDate);
            DateTime newStartDate = DateTime.ParseExact(string.Join(".", new string[] { "15", "10", "2050", "14", "30" }), "d.M.yyyy.HH.mm", null);

            // Act
            executionDate.ChangeDateStart(newStartDate);

            // Assert
            Assert.AreEqual(DateTime.ParseExact("15.10.2050 14:30", "d.M.yyyy HH:mm", null), executionDate.DateStartTask);
        }

        [TestMethod]
        public void ChangeDateEnd_ValidInput()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask, Repeat.None, StartDate, EndDate);
            DateTime newEndDate = DateTime.ParseExact(string.Join(".", new string[] { "30", "10", "2050", "15", "30" }), "d.M.yyyy.HH.mm", null);

            // Act
            executionDate.ChangeDateEnd(newEndDate);

            // Assert
            Assert.AreEqual(DateTime.ParseExact("30.10.2050 15:30", "d.M.yyyy HH:mm", null), executionDate.DateEndTask);
        }

        [TestMethod]
        public void ChangeRepeat_ValidInput()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask, Repeat.None, StartDate, EndDate);
            Repeat repeat = Repeat.EveryMonth;

            // Act
            executionDate.ChangeRepeat(repeat);

            // Assert
            Assert.AreEqual(Repeat.EveryMonth, executionDate.OftenRepeat);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void ChangeDateStart_InValidInput_ThrowsException()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask, Repeat.None, StartDate, EndDate);
            DateTime newStartDate = DateTime.ParseExact(string.Join(".", new string[] { "18", "10", "2050", "14", "30" }), "d.M.yyyy.HH.mm", null);

            // Act
            Assert.ThrowsException<Exception>(() => executionDate.ChangeDateStart(newStartDate));
        }

        [TestMethod]
        public void ChangeDateEnd_InValidInput_ThrowsException()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask, Repeat.None, StartDate, EndDate);
            DateTime newEndDate = DateTime.ParseExact(string.Join(".", new string[] { "9", "10", "2050", "15", "30" }), "d.M.yyyy.HH.mm", null);

            // Act
            Assert.ThrowsException<Exception>(() => executionDate.ChangeDateEnd(newEndDate));
        }

        [TestMethod]
        public void ChangeRepeat_InValidInput_ThrowsException()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask, Repeat.None, StartDate, EndDate);
            Repeat repeat = Repeat.Everyday;

            // Act
            Assert.ThrowsException<Exception>(() => executionDate.ChangeRepeat(repeat));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Checked_TaskIsExpectation()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask);

            // Act
            executionDate.Checked();

            // Assert
            Assert.AreEqual(State.Expectation, executionDate.StateTask);
        }

        [TestMethod]
        public void Checked_TaskIsInProcess()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask);
            executionDate.ChangeDateStart(DateTime.Now);

            // Act
            executionDate.Checked();

            // Assert
            Assert.AreEqual(State.InProcess, executionDate.StateTask);
        }

        [TestMethod]
        public void Checked_TaskIsOverdue()
        {
            // Arrange
            ExecutionDate executionDate = new(TestTask, Repeat.None, DateTime.Now, DateTime.Now);

            // Act
            executionDate.Checked();

            // Assert
            Assert.AreEqual(State.Overdue, executionDate.StateTask);
        }

    }
}
