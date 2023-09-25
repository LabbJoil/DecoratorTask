
#### ��������� ��� ����� � Visual Studio

������ ������ � Visual Studio ��������� ���������� �������� "���������" � ����� ���������������� C# ��� ���������� ������� ����� ���������� ���������� � �������������� �����������������. �������� ���� ������� - ��������, ��� ����� ��������� ���������������� ������� ������� �����, ����� �� ����� ������� � ��������������.

## �������� ������

### BasicTask

BasicTask - ��� ������� �����, ������� ������������ ����� ������� ������. �� �������� � ���� ��������� ��������:

- Title - ��������� ������.
- Description - �������� ������.
- StateTask - ������� ��������� ������ (��������, "��������", "� ��������", "���������").

�������� ������ BasicTask ��������:

- GetId() - ��������� ����������� �������������� ������.
- Info() - �������������� ���������� � ������.
- DeleteTask(ref ITask deleteTask) - �������� ������.
- ChangeTask(ITask lastTask, ITask newTask) - ��������� ������.

### CustomTask

CustomTask - ��� ���������, ������� ��������� �������������� �������� � ���������������� � �������. �� �������� ��������� ��������:

- ConditionPriority - ��������� ������ (��������, "������", "�����������", "�������").
- IsArchived - ����, �����������, �������� �� ������ ��������.

CustomTask ����� ������������� ����������� ������ ��� ���������� ����� �� ��������� ���������, ����� ��� ���������, ��������� � �������� ���������.

### ExecutionDate

ExecutionDate - ��� ���� ��������� ��� �����, ������� ��������� ����������� �������� ���� ������ � ��������� ���������� ������, � ����� ������������� ����������. ���� ��������� �������� ��������� ��������:

- DateStartTask - ���� � ����� ������ ���������� ������.
- DateEndTask - ���� � ����� ��������� ���������� ������.
- OftenRepeat - ������������� ���������� ������ (��������, "������ ����", "������ ������").

ExecutionDate ����� ������������� ������ ��� ��������� ��� � ������� ������ � ��������� ���������� ������, � ����� ������������� ����������. ����� Checked() ������������ ��� �������� ��������� ������ �� ������ ��������� ��� � �������.

### TaskEnhancer

TaskEnhancer - ��� ����������� ������� ����� ��� ���� ����������� �����. �� �������� � ���� ����� �������� � ������, ����� ��� Title, Description, StateTask, GetId(), Info(), DeleteTask(ref ITask deleteTask) � ChangeTask(ITask lastTask, ITask newTask).

## ��������� ITask

ITask - ��� ���������, ������� ���������� ����� �������� � ������ ��� ���� ������� �����. �� �������� � ���� Title, Description, StateTask, GetId(), Info(), DeleteTask(ref ITask deleteTask) � ChangeTask(ITask lastTask, ITask newTask).


## ������� �������������

```csharp

using DecoratorTask.Decorators;
using DecoratorTask.Entities;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

// �������� ������� ������
ITask task = new BasicTask("My Task", "description about my task", State.InProcess);

// ���������� �������������� ��������� � ������
task = new CustomTask(task, Priority.Priority);

// ���������� �������� � ������ � ������� ����������
task = new ExecutionDate(task, Repeat.Everyday, DateTime.Now.AddHours(3), DateTime.Now.AddHours(6));
