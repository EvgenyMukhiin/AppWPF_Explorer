using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TestApp.Infrastructure.Commands.Base
{
    internal class ActionCommand : Command
    {

        private readonly Action<object> _Execute;   //readonly - работают быстрее
        private readonly Func<object, bool> _CanExecute;

        public ActionCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            _Execute = Execute ?? throw new ArgumentException(nameof(Execute));
            _CanExecute = CanExecute;
        }

        public override bool CanExecute(object parametr) => _CanExecute?.Invoke(parametr) ?? true;

        public override void Execute(object parametr) => _Execute(parametr);

    }
}
