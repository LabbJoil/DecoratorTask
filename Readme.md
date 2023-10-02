# ��������� ��� �����

������ ������ � Visual Studio ��������� ���������� �������� "���������" � ����� ���������������� C# ��� ���������� ������� ����� ���������� ���������� � �������������� �����������������. �������� ���� ������� - ��������, ��� ����� ��������� ���������������� ������� ������� �����, ����� �� ����� ������� � ��������������.

## �������� ������

### BasicTask

BasicTask - ��� ������� �����, ������� ������������ ����� ������� ������. �� �������� � ���� ��������� ��������:

- Title - ��������� ������.
- Description - �������� ������.
- StateTask - ������� ��������� ������ (��������, "��������", "� ��������", "���������").

�������� ������ BasicTask ��������:

- GetId() - ��������� ����������� �������������� ������.
- GetState() - ��������� �������� ��������� ������.
- Info() - �������������� ���������� � ������.
- DeleateTask(ref ITask deleteTask) - �������� ������.
- CompleteTask() - ������� ������ ��� �����������.
- ChangeTask(ITask lastTask, ITask newTask) - ��������� ������ � ������ ����� �� ����� ������.

### CustomTask

CustomTask - ��� ���������, ������� ��������� �������������� �������� � ���������������� � �������. �� �������� ��������� ��������:

- ConditionPriority - ��������� ������ (��������, "������", "�����������", "�������").
- IsArchived - ����, �����������, �������� �� ������ ��������.
- ArchivedFilePath - ���� � ����� �������������� ������ (����� ���� `null`).

CustomTask ����� ������������� ����������� ������ ��� ���������� ����� �� ��������� ���������, ����� ��� ���������, ��������� � �������� ���������.

### ExecutionDate

ExecutionDate - ��� ���� ��������� ��� �����, ������� ��������� ����������� �������� ���� ������ � ��������� ���������� ������, � ����� ������������� ����������. ���� ��������� �������� ��������� ��������:

- DateStartTask - ���� � ����� ������ ���������� ������.
- DateEndTask - ���� � ����� ��������� ���������� ������.
- OftenRepeat - ������������� ���������� ������ (��������, "������ ����", "������ ������").

ExecutionDate ����� ������������� ������ ��� ��������� ��� � ������� ������ � ��������� ���������� ������, � ����� ������������� ����������. ����� Checked() ������������ ��� �������� ��������� ������ �� ������ ��������� ��� � �������.

### TaskEnhancer

TaskEnhancer - ��� ����������� ������� ����� ��� ���� ����������� �����. �� �������� � ���� ����� �������� � ������, ����� ��� Title, Description, StateTask, GetId(), GetState(), Info(), DeleateTask(ref ITask deleteTask), CompleteTask() � ChangeTask(ITask lastTask, ITask newTask).

## ��������� ITask

ITask - ��� ���������, ������� ���������� ����� �������� � ������ ��� ���� ������� �����. �� �������� � ���� Title, Description, StateTask, GetId(), GetState(), Info(), DeleateTask(ref ITask deleteTask), CompleteTask() � ChangeTask(ITask lastTask, ITask newTask).

## ������� �������������

```csharp
using DecoratorTask.Decorators;
using DecoratorTask.Entities;
using DecoratorTask.Enums;
using DecoratorTask.Interfaces;

// �������� ������� ������
ITask task = new BasicTask("My Task", "description about my task", State.InProcess);

// ���������� �������������� ��������� � ������
task = new CustomTask(ref task, Priority.Priority);

// ���������� �������� � ������ � ������� ����������
task = new ExecutionDate(ref task, Repeat.Everyday, DateTime.Now.AddHours(3), DateTime.Now.AddHours(6));
