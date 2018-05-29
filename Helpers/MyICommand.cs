﻿using System;
using System.Windows.Input;

namespace ToDoList.Helpers
{
    /// <summary>
    /// Klasa reprezentująca własną implementację interfejsu ICommand
    /// </summary>
    public class MyICommand : ICommand
    {
        private Action _TargetExecuteMethod;
        private Func<bool> _TargetCanExecuteMethod;

        public event EventHandler CanExecuteChanged = delegate { };

        public MyICommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public MyICommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object par)
        {
            if (_TargetCanExecuteMethod != null)
                return _TargetCanExecuteMethod();

            if (_TargetExecuteMethod != null)
                return true;

            return false;
        }

        public void Execute(object par)
        {
            _TargetExecuteMethod?.Invoke();
        }
    }


    public class MyICommand<T> : ICommand
    {
        private Action<T> _TargetExecuteMethod;
        private Func<T, bool> _TargetCanExecuteMethod;

        public event EventHandler CanExecuteChanged = delegate { };

        public MyICommand(Action<T> executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public MyICommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        public bool CanExecute(object par)
        {
            if (_TargetCanExecuteMethod != null)
            {
                T tpar = (T)par;
                return _TargetCanExecuteMethod(tpar);
            }

            if (_TargetExecuteMethod != null)
                return true;

            return false;
        }

        public void Execute(object par)
        {
            _TargetExecuteMethod?.Invoke((T)par);
        }
    }
}
