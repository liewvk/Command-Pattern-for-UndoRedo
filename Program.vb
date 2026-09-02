Imports System
Imports System.Collections.Generic

' ============================================================
' Student Model
' ============================================================

Public Class Student

    Public Property StudentID As Integer
    Public Property Name As String
    Public Property Course As String

End Class


' ============================================================
' Repository Interface
' ============================================================

Public Interface IStudentRepository

    Sub Add(student As Student)

    Sub Delete(studentId As Integer)

    Function GetAll() As List(Of Student)

End Interface


' ============================================================
' In-Memory Repository
' ============================================================

Public Class StudentRepository
    Implements IStudentRepository

    Private ReadOnly _students As New List(Of Student)

    Private _nextId As Integer = 1


    Public Sub Add(student As Student) _
        Implements IStudentRepository.Add

        student.StudentID = _nextId
        _nextId += 1

        _students.Add(student)

    End Sub


    Public Sub Delete(studentId As Integer) _
        Implements IStudentRepository.Delete

        Dim student As Student =
            _students.Find(
                Function(s) s.StudentID = studentId)

        If student IsNot Nothing Then
            _students.Remove(student)
        End If

    End Sub


    Public Function GetAll() As List(Of Student) _
        Implements IStudentRepository.GetAll

        Return New List(Of Student)(_students)

    End Function

End Class


' ============================================================
' Command Interface
'
' Every command must know how to:
'   1. Execute itself
'   2. Undo itself
' ============================================================

Public Interface ICommand

    Sub Execute()

    Sub Unexecute()

    ReadOnly Property Description As String

End Interface


' ============================================================
' Concrete Command - Add Student
' ============================================================

Public Class AddStudentCommand
    Implements ICommand

    Private ReadOnly _repository As IStudentRepository
    Private ReadOnly _student As Student

    Private _assignedId As Integer


    Public Sub New(
        repository As IStudentRepository,
        student As Student)

        _repository = repository
        _student = student

    End Sub


    ' --------------------------------------------------------
    ' Execute
    ' --------------------------------------------------------

    Public Sub Execute() _
        Implements ICommand.Execute

        _repository.Add(_student)

        _assignedId =
            _student.StudentID

    End Sub


    ' --------------------------------------------------------
    ' Undo
    ' --------------------------------------------------------

    Public Sub Unexecute() _
        Implements ICommand.Unexecute

        _repository.Delete(_assignedId)

    End Sub


    ' --------------------------------------------------------
    ' Description
    ' --------------------------------------------------------

    Public ReadOnly Property Description As String _
        Implements ICommand.Description

        Get
            Return "Add " & _student.Name
        End Get

    End Property

End Class


' ============================================================
' Command Manager
'
' Stores executed commands in two stacks:
'
' Undo Stack
' Redo Stack
' ============================================================

Public Class CommandManager

    Private ReadOnly _undo As New Stack(Of ICommand)

    Private ReadOnly _redo As New Stack(Of ICommand)


    ' --------------------------------------------------------
    ' Execute a new command
    ' --------------------------------------------------------

    Public Sub Execute(command As ICommand)

        command.Execute()

        _undo.Push(command)

        ' A new command invalidates the old redo history
        _redo.Clear()

    End Sub


    ' --------------------------------------------------------
    ' Undo most recent command
    ' --------------------------------------------------------

    Public Sub Undo()

        If _undo.Count = 0 Then

            Console.WriteLine(
                "Nothing to undo.")

            Return

        End If


        Dim command As ICommand =
            _undo.Pop()

        command.Unexecute()

        _redo.Push(command)

        Console.WriteLine(
            "Undone: " &
            command.Description)

    End Sub


    ' --------------------------------------------------------
    ' Redo most recently undone command
    ' --------------------------------------------------------

    Public Sub Redo()

        If _redo.Count = 0 Then

            Console.WriteLine(
                "Nothing to redo.")

            Return

        End If


        Dim command As ICommand =
            _redo.Pop()

        command.Execute()

        _undo.Push(command)

        Console.WriteLine(
            "Redone: " &
            command.Description)

    End Sub


    ' --------------------------------------------------------
    ' Status Properties
    ' --------------------------------------------------------

    Public ReadOnly Property CanUndo As Boolean

        Get
            Return _undo.Count > 0
        End Get

    End Property


    Public ReadOnly Property CanRedo As Boolean

        Get
            Return _redo.Count > 0
        End Get

    End Property


    Public ReadOnly Property UndoDescription As String

        Get

            If _undo.Count > 0 Then
                Return _undo.Peek().Description
            End If

            Return ""

        End Get

    End Property


    Public ReadOnly Property RedoDescription As String

        Get

            If _redo.Count > 0 Then
                Return _redo.Peek().Description
            End If

            Return ""

        End Get

    End Property

End Class


' ============================================================
' Main Program
' ============================================================

Module Program

    Sub Main()

        Dim repository As New StudentRepository

        Dim manager As New CommandManager


        Console.WriteLine(
            "=== Command Pattern - Undo / Redo Demo ===")

        Console.WriteLine()


        ' ====================================================
        ' Add first student
        ' ====================================================

        Dim student1 As New Student With {
            .Name = "Emma Wilson",
            .Course = "Computer Science"
        }


        Dim command1 As New AddStudentCommand(
            repository,
            student1)


        Console.WriteLine(
            "Executing: " &
            command1.Description)

        manager.Execute(command1)

        DisplayStudents(repository)


        ' ====================================================
        ' Add second student
        ' ====================================================

        Dim student2 As New Student With {
            .Name = "Daniel Brown",
            .Course = "Data Science"
        }


        Dim command2 As New AddStudentCommand(
            repository,
            student2)


        Console.WriteLine()

        Console.WriteLine(
            "Executing: " &
            command2.Description)

        manager.Execute(command2)

        DisplayStudents(repository)


        ' ====================================================
        ' Add third student
        ' ====================================================

        Dim student3 As New Student With {
            .Name = "Sophie Taylor",
            .Course = "Artificial Intelligence"
        }


        Dim command3 As New AddStudentCommand(
            repository,
            student3)


        Console.WriteLine()

        Console.WriteLine(
            "Executing: " &
            command3.Description)

        manager.Execute(command3)

        DisplayStudents(repository)


        ' ====================================================
        ' Show Undo information
        ' ====================================================

        Console.WriteLine()

        Console.WriteLine(
            "Next Undo: " &
            manager.UndoDescription)


        ' ====================================================
        ' Undo third student
        ' ====================================================

        Console.WriteLine()
        Console.WriteLine("--- UNDO ---")

        manager.Undo()

        DisplayStudents(repository)


        ' ====================================================
        ' Undo second student
        ' ====================================================

        Console.WriteLine()
        Console.WriteLine("--- UNDO AGAIN ---")

        manager.Undo()

        DisplayStudents(repository)


        ' ====================================================
        ' Redo second student
        ' ====================================================

        Console.WriteLine()
        Console.WriteLine("--- REDO ---")

        manager.Redo()

        DisplayStudents(repository)


        ' ====================================================
        ' Redo third student
        ' ====================================================

        Console.WriteLine()
        Console.WriteLine("--- REDO AGAIN ---")

        manager.Redo()

        DisplayStudents(repository)


        Console.WriteLine()
        Console.WriteLine(
            "Press any key to exit...")

        Console.ReadKey()

    End Sub


    ' ============================================================
    ' Display students currently stored
    ' ============================================================

    Private Sub DisplayStudents(
        repository As IStudentRepository)

        Console.WriteLine()
        Console.WriteLine(
            "Current students:")

        Dim students As List(Of Student) =
            repository.GetAll()


        If students.Count = 0 Then

            Console.WriteLine(
                "  No students.")

            Return

        End If


        For Each student As Student In students

            Console.WriteLine(
                $"  {student.StudentID}: " &
                $"{student.Name} - " &
                $"{student.Course}")

        Next

    End Sub

End Module
