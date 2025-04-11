using System.Windows.Input;

namespace Agriventory;

public class RelayCommand : ICommand
{
   private Action<object> _execute;
   private Func<object, bool> canExecute;

   public RelayCommand(Action<object> execute)
   {
      this._execute = execute;
      canExecute = null;
   }

   public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
   {
      this._execute = execute;
      this.canExecute = canExecute;
   }
   
   public event EventHandler CanExecuteChanged
   {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
   }

   public bool CanExecute(object parameter)
   {
      return canExecute == null || CanExecute(parameter);
   }

   public void Execute(object parameter)
   {
      _execute(parameter);
   }
   
   
}