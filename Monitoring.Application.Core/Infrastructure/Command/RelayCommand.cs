﻿using System;
using SystemMonitoringNetCore.Infrastructure.Command.Base;

namespace SystemMonitoringNetCore.Infrastructure.Command;

public class RelayCommand : BaseCommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool>? _canExecute;
    public RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }
    
    public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

    public override void Execute(object parameter) => _execute(parameter);
}