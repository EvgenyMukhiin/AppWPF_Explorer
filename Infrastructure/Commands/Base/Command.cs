using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_TestApp.Infrastructure.Commands.Base
{
    internal abstract class Command : ICommand
    {

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public abstract bool CanExecute(object parametr);  //возвращается либо истина либо ложь и команду выполнить нельзя

        public abstract void Execute(object parametr);   //то что должно быть выполнено командой
    }
}
