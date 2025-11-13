# Core Logic Separation Architecture

## Overview

The RC41 calculator emulator has been refactored to completely separate the core calculator logic from the UI layer. This allows the UI to be easily swapped for different implementations without modifying any calculator logic.

## Architecture

### Core Interfaces (`Rc41/Core/Interfaces/`)

All communication between the core logic and UI happens through these interfaces:

- **ICalculatorUI** - Main interface combining all UI operations
- **IDisplayAdapter** - Display and printer output operations
- **IAnnunciatorAdapter** - Calculator indicator/annunciator controls
- **ITimerAdapter** - Timer control operations
- **IPrinterAdapter** - Printer configuration queries
- **IDebugAdapter** - Debug and trace operations
- **ICardReaderAdapter** - Card reader dialog operations

### Core Logic Modules

These modules contain no direct UI dependencies and only use the interfaces:

- **Cpu** (`Rc41/Cpu/`) - Main calculator CPU emulation
- **Printer** (`Rc41/Printer/`) - Printer peripheral
- **TapeDrive** (`Rc41/TapeDrive/`) - Tape drive peripheral
- **CardReader** (`Rc41/CardReader/`) - Card reader peripheral
- **Extended** (`Rc41/Extended/`) - Extended memory module
- **TimeModule** (`Rc41/TimeModule/`) - Time/stopwatch module
- **Ui** (`Rc41/Ui.cs`) - User input handler
- **Debugger** (`Rc41/Debugger.cs`) - Debug utilities

### UI Implementation

- **Form1** (`Rc41/Form1.cs`) - Windows Forms implementation of ICalculatorUI

The Form1 class implements all required interfaces and handles the actual UI rendering and user interaction.

## Benefits

1. **Testability** - Core logic can be tested without UI dependencies
2. **Flexibility** - UI can be replaced (console, web, mobile, etc.)
3. **Maintainability** - Clear separation of concerns
4. **Reusability** - Core logic can be reused in different contexts

## Migration Guide

To create an alternative UI implementation:

1. Implement the `ICalculatorUI` interface (and all its base interfaces)
2. Create instances of core modules (Cpu, Ui, Debugger) passing your implementation
3. Handle user input by calling methods on Ui and Cpu
4. Respond to callbacks through your interface implementations

Example:
```csharp
public class ConsoleUI : ICalculatorUI
{
    public void Display(string message, bool scroll)
    {
        Console.WriteLine(message);
    }
    
    // Implement other interface methods...
}

// Usage
var ui = new ConsoleUI();
var cpu = new Cpu(ui);
var inputHandler = new Ui(cpu, ui);
// ...
```
