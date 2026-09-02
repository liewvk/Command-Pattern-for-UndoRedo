# Command Pattern for Undo/Redo

A Visual Basic .NET implementation demonstrating the **Command Pattern** design pattern with practical **Undo** and **Redo** functionality.

## 📋 Overview

This project showcases how to implement the Command Pattern to create a robust system that allows users to undo and redo operations. The example uses a student management system where users can add students, and then undo/redo those operations.

## 🎯 Design Pattern Explanation

### What is the Command Pattern?

The Command Pattern is a behavioral design pattern that encapsulates a request as an object, allowing you to:
- Parameterize clients with different requests
- Queue requests
- Log requests
- Support undo/redo operations

### Key Components

1. **ICommand Interface** - Defines the contract for all commands
   - `Execute()` - Performs the command action
   - `Unexecute()` - Reverses the command action (undo)
   - `Description` - Describes what the command does

2. **Concrete Commands** - Implement specific operations
   - `AddStudentCommand` - Implements adding a student with undo capability

3. **Command Manager** - Orchestrates command execution and history
   - Maintains undo and redo stacks
   - Manages command execution flow
   - Provides undo/redo functionality

4. **Repository** - Manages the data (student storage)
   - `IStudentRepository` - Interface for data operations
   - `StudentRepository` - In-memory implementation

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Main Application                     │
└──────────────────────┬──────────────────────────────────┘
                       │
        ┌──────────────┼──────────────┐
        │              │              │
    ┌───▼──────┐  ┌────▼──────┐  ┌──▼───────┐
    │CommandMgr│  │Repository │  │ Commands  │
    └────┬─────┘  └───────────┘  └──────────┘
         │
    ┌────┴─────┬────────┐
    │           │        │
┌───▼──┐  ┌────▼──┐  ┌──▼────┐
│Undo  │  │Redo   │  │Execute │
│Stack │  │Stack  │  │Command │
└──────┘  └───────┘  └────────┘
```

## 🚀 Features

- ✅ **Execute Commands** - Add students to the repository
- ✅ **Undo Operations** - Reverse the last command
- ✅ **Redo Operations** - Restore an undone command
- ✅ **Command History** - Track executable and reversible commands
- ✅ **Descriptive Actions** - Display what command will be undone/redone
- ✅ **State Management** - Check if undo/redo is available

## 📁 Project Structure

```
Command-Pattern-for-UndoRedo/
├── Program.vb                           # Main application file
├── Command Pattern for UndoRedo.vbproj  # Project configuration
├── Command Pattern for UndoRedo.slnx    # Solution file
├── .gitignore                           # Git ignore rules
├── .gitattributes                       # Git attributes
└── README.md                            # This file
```

## 💻 Code Components

### Student Model
```vb
Public Class Student
    Public Property StudentID As Integer
    Public Property Name As String
    Public Property Course As String
End Class
```

### ICommand Interface
Defines the command contract with Execute, Unexecute, and Description.

### AddStudentCommand
Concrete implementation that:
- Stores a reference to the repository and student
- Captures the assigned ID during execution
- Implements undo by deleting using the stored ID

### CommandManager
Manages two stacks:
- **Undo Stack** - Stores executed commands
- **Redo Stack** - Stores undone commands

Key methods:
- `Execute(command)` - Execute a command and push to undo stack
- `Undo()` - Pop from undo stack, unexecute, and push to redo stack
- `Redo()` - Pop from redo stack, execute, and push to undo stack

## 🔄 How It Works

### Execution Flow

1. **Execute Command**
   ```
   Command → Execute() → Push to Undo Stack → Clear Redo Stack
   ```

2. **Undo Command**
   ```
   Pop from Undo Stack → Unexecute() → Push to Redo Stack
   ```

3. **Redo Command**
   ```
   Pop from Redo Stack → Execute() → Push to Undo Stack
   ```

## 📊 Demo Execution

The program demonstrates:

1. Adding three students:
   - Emma Wilson (Computer Science)
   - Daniel Brown (Data Science)
   - Sophie Taylor (Artificial Intelligence)

2. Undoing twice (removing the last two students)

3. Redoing twice (re-adding the two students)

The console output shows:
- Current student list after each operation
- Description of each undo/redo action
- What commands are available to undo/redo

## 🎓 Learning Outcomes

This implementation teaches:

- How to implement the **Command Pattern**
- Stack-based undo/redo mechanisms
- Encapsulation of operations
- Separation of concerns (commands, repository, manager)
- State preservation for reversible operations
- Interface-driven design in Visual Basic .NET

## 🛠️ Building & Running

### Prerequisites
- Visual Studio 2022 or later
- .NET Framework 4.7.2+ or .NET Core/5+
- Visual Basic .NET support

### Build
```bash
dotnet build "Command Pattern for UndoRedo.vbproj"
```

### Run
```bash
dotnet run --project "Command Pattern for UndoRedo.vbproj"
```

Or directly from Visual Studio:
- Open the solution file
- Press F5 to build and run

## 📈 Extension Ideas

To extend this project, consider:

1. **Additional Commands** - Implement `DeleteStudentCommand`, `UpdateStudentCommand`
2. **Command Macros** - Group multiple commands into a single undo/redo
3. **Persistence** - Save command history to disk
4. **Command Limitations** - Add size limits to undo/redo stacks
5. **GUI** - Add a Windows Forms interface with Undo/Redo buttons
6. **Command Timing** - Add timestamps to track when commands were executed
7. **Error Handling** - Implement exception handling in command execution

## 📚 Design Pattern Benefits

- **Loose Coupling** - Commands decouple sender and receiver
- **Flexibility** - Easy to add new commands without changing existing code
- **Testability** - Commands can be tested independently
- **Reusability** - Commands can be used in different contexts
- **Extensibility** - New functionality can be added through new command classes

## 📝 License

This project is provided as an educational example.

## 👨‍💻 Author

Created for Visual Basic 2026 Programming course - Lesson 39: Design Patterns

---

**Happy Coding!** 🚀
