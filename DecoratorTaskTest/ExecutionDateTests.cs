﻿using DecoratorTask.Decorators;
using DecoratorTask.Entities;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

namespace DecoratorTaskTest;

[TestClass]
public class ExecutionDateTests
{
    private readonly DateTime StartDate = DateTime.ParseExact("10.10.2050 14:30", "d.M.yyyy HH:mm", null);
    private readonly DateTime EndDate = DateTime.ParseExact("16.10.2050 15:30", "d.M.yyyy HH:mm", null);

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void Constructor_DefaultValues()
    {
        // Arrange
        DateTime nowDateTime = DateTime.Now;
        DateTime dateStartTask = new DateTime(nowDateTime.Year, nowDateTime.Month, nowDateTime.Day, nowDateTime.Hour, nowDateTime.Minute, 0).AddHours(1);
        DateTime dateEndTask = dateStartTask.AddHours(1);

        // Act
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask);

        // Assert
        Assert.AreEqual(dateStartTask.Hour, executionDate.DateStartTask.Hour);
        Assert.AreEqual(dateEndTask.Hour, executionDate.DateEndTask.Hour);
        Assert.AreEqual(Repeat.None, executionDate.OftenRepeat);
    }

    [TestMethod]
    public void Constructor_WithValues()
    {
        // Arrange
        Repeat repeat = Repeat.EveryWeek;

        // Act
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, repeat, StartDate, EndDate);

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
        Repeat repeat = Repeat.EveryDay;
        ITask? basicTask = new BasicTask();

        // Act & Assert
        Assert.ThrowsException<Exception>(() 
            => new ExecutionDate( basicTask, repeat, StartDate, EndDate));
    }

    [TestMethod]
    public void Constructor_InValidStartDateInput_ThrowsException()
    {
        // Arrange
        Repeat repeat = Repeat.None;
        DateTime IncorrectStartDate = DateTime.ParseExact("17.10.2050 15:30", "d.M.yyyy HH:mm", null);

        // Act & Assert
        ITask? basicTask = new BasicTask();
        Assert.ThrowsException<Exception>(() => new ExecutionDate( basicTask, repeat, IncorrectStartDate, EndDate));
    }

    [TestMethod]
    public void Constructor_InValidEndDateInput_ThrowsException()
    {
        // Arrange
        Repeat repeat = Repeat.None;
        DateTime IncorrectEndDate = DateTime.ParseExact("09.10.2050 15:30", "d.M.yyyy HH:mm", null);

        // Act & Assert
        ITask? basicTask = new BasicTask();
        Assert.ThrowsException<Exception>(() => new ExecutionDate( basicTask, repeat, StartDate, IncorrectEndDate));
    }

    [TestMethod]
    public void Constructor_InValidDateInput_ThrowsException()
    {
        // Arrange
        Repeat repeat = Repeat.None;
        ITask? basicTask = new BasicTask();
        DateTime IncorrectEndDate = DateTime.ParseExact("09.09.2012 15:30", "d.M.yyyy HH:mm", null);

        // Act & Assert
        Assert.ThrowsException<Exception>(() => new ExecutionDate( basicTask, repeat, StartDate, IncorrectEndDate));
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void ChangeDateStart_ValidInput()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.None, StartDate, EndDate);
        DateTime newStartDate = DateTime.ParseExact("15.10.2050 14:30", "d.M.yyyy HH:mm", null);

        // Act
        executionDate.ChangeDateStart(newStartDate);

        // Assert
        Assert.AreEqual(DateTime.ParseExact("15.10.2050 14:30", "d.M.yyyy HH:mm", null), executionDate.DateStartTask);
    }

    [TestMethod]
    public void ChangeDateEnd_ValidInput()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.None, StartDate, EndDate);
        DateTime newEndDate = DateTime.ParseExact("30.10.2050 15:30", "d.M.yyyy HH:mm", null);

        // Act
        executionDate.ChangeDateEnd(newEndDate);

        // Assert
        Assert.AreEqual(DateTime.ParseExact("30.10.2050 15:30", "d.M.yyyy HH:mm", null), executionDate.DateEndTask);
    }

    [TestMethod]
    public void ChangeRepeat_ValidInput()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.None, StartDate, EndDate);
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
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.None, StartDate, EndDate);
        DateTime newStartDate = DateTime.ParseExact("18.10.2050 14:30", "d.M.yyyy HH:mm", null);

        // Act & Assert
        Assert.ThrowsException<Exception>(() => executionDate.ChangeDateStart(newStartDate));
    }

    [TestMethod]
    public void ChangeDateEnd_InValidInput_ThrowsException()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.None, StartDate, EndDate);
        DateTime newEndDate = DateTime.ParseExact("09.10.2050 15:30", "d.M.yyyy HH:mm", null);

        // Act & Assert
        Assert.ThrowsException<Exception>(() => executionDate.ChangeDateEnd(newEndDate));
    }

    [TestMethod]
    public void ChangeRepeat_InValidInput_ThrowsException()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.None, StartDate, EndDate);
        Repeat repeat = Repeat.EveryDay;

        // Act & Assert
        Assert.ThrowsException<Exception>(() => executionDate.ChangeRepeat(repeat));
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void Checked_TaskIsExpectation()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask);

        // Act
        executionDate.Checked();

        // Assert
        Assert.AreEqual(State.Expectation, executionDate.StateTask);
    }

    [TestMethod]
    public void Checked_TaskIsInProcess()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.EveryMonth, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(15));

        // Act
        executionDate.Checked();

        // Assert
        Assert.AreEqual(State.InProcess, executionDate.StateTask);
    }

    [TestMethod]
    public void Checked_TaskIsComplete ()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.None, DateTime.Now, DateTime.Now);

        // Act
        executionDate.Checked();

        // Assert
        Assert.AreEqual(State.Complete, executionDate.StateTask);
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------

    [TestMethod]
    public void CompleteTask_TaskIsExpectation()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.EveryMonth, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5));

        // Act
        executionDate.CompleteTask();

        // Assert
        Assert.AreEqual(State.Expectation, executionDate.StateTask);
    }

    [TestMethod]
    public void CompleteTask_TaskIsComplete()
    {
        // Arrange
        ITask? basicTask = new BasicTask();
        ExecutionDate executionDate = new( basicTask, Repeat.None, DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5));

        // Act
        executionDate.CompleteTask();

        // Assert
        Assert.AreEqual(State.Complete, executionDate.StateTask);
    }
}
