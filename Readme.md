# Декоратор для задач

Данный проект в Visual Studio исследует применение паттерна "Декоратор" в языке программирования C# для обогащения базовых задач различными атрибутами и дополнительной функциональностью. Основная цель проекта - показать, как можно расширять функциональность базовых классов задач, делая их более гибкими и настраиваемыми.

## Основные классы

### BasicTask

BasicTask - это базовый класс, который представляет собой простую задачу. Он включает в себя следующие атрибуты:

- Title - заголовок задачи.
- Description - описание задачи.
- StateTask - текущее состояние задачи (например, "ожидание", "в процессе", "завершено").

Основные методы BasicTask включают:

- GetId() - получение уникального идентификатора задачи.
- GetState() - получение текущего состояния задачи.
- Info() - предоставление информации о задаче.
- DeleateTask(ref ITask deleteTask) - удаление задачи.
- CompleteTask() - пометка задачи как завершенной.
- ChangeTask(ITask lastTask, ITask newTask) - изменение задачи в списке задач на более свежую.

### CustomTask

CustomTask - это декоратор, который добавляет дополнительные атрибуты и функциональность к задачам. Он включает следующие атрибуты:

- ConditionPriority - приоритет задачи (например, "низкий", "стандартный", "высокий").
- IsArchived - флаг, указывающий, является ли задача архивной.
- ArchivedFilePath - путь к файлу архивированной задачи (может быть `null`).

CustomTask также предоставляет статические методы для фильтрации задач по различным критериям, таким как приоритет, состояние и архивное состояние.

### ExecutionDate

ExecutionDate - еще один декоратор для задач, который добавляет возможность указания даты начала и окончания выполнения задачи, а также периодичность повторения. Этот декоратор включает следующие атрибуты:

- DateStartTask - дата и время начала выполнения задачи.
- DateEndTask - дата и время окончания выполнения задачи.
- OftenRepeat - периодичность повторения задачи (например, "каждый день", "каждую неделю").

ExecutionDate также предоставляет методы для изменения дат и времени начала и окончания выполнения задачи, а также периодичности повторения. Метод Checked() используется для проверки состояния задачи на основе указанных дат и времени.

### TaskEnhancer

TaskEnhancer - это абстрактный базовый класс для всех декораторов задач. Он включает в себя общие атрибуты и методы, такие как Title, Description, StateTask, GetId(), GetState(), Info(), DeleateTask(ref ITask deleteTask), CompleteTask() и ChangeTask(ITask lastTask, ITask newTask).

## Интерфейс ITask

ITask - это интерфейс, который определяет общие атрибуты и методы для всех классов задач. Он включает в себя Title, Description, StateTask, GetId(), GetState(), Info(), DeleateTask(ref ITask deleteTask), CompleteTask() и ChangeTask(ITask lastTask, ITask newTask).

## Примеры использования

```csharp
using DecoratorTask.Decorators;
using DecoratorTask.Entities;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

// Создание базовой задачи
ITask task = new BasicTask("My Task", "description about my task", State.InProcess);

// Добавление дополнительных атрибутов к задаче
task = new CustomTask(ref task, Priority.Priority);

// Добавление дедлайна к задаче и частоты повторений
task = new ExecutionDate(ref task, Repeat.Everyday, DateTime.Now.AddHours(3), DateTime.Now.AddHours(6));
