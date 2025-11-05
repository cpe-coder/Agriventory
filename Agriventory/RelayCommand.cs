using System.Windows.Input;

namespace Agriventory;

public class RelayCommand : ICommand
{
    private readonly Func<object, Task> _executeAsync;
    private readonly Action<object> _executeSync;
    private readonly Predicate<object> _canExecute;

    // Async constructor (object parameter)
    public RelayCommand(Func<object, Task> executeAsync, Predicate<object> canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
    }

    // Sync constructor (object parameter)
    public RelayCommand(Action<object> executeSync, Predicate<object> canExecute = null)
    {
        _executeSync = executeSync ?? throw new ArgumentNullException(nameof(executeSync));
        _canExecute = canExecute;
    }

    // Convenience constructor for parameterless async
    public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
    {
        if (executeAsync == null) throw new ArgumentNullException(nameof(executeAsync));
        _executeAsync = _ => executeAsync();
        _canExecute = canExecute == null ? (Predicate<object>)null : new Predicate<object>(_ => canExecute());
    }

    // Convenience constructor for parameterless sync
    public RelayCommand(Action executeSync, Func<bool> canExecute = null)
    {
        if (executeSync == null) throw new ArgumentNullException(nameof(executeSync));
        _executeSync = _ => executeSync();
        _canExecute = canExecute == null ? (Predicate<object>)null : new Predicate<object>(_ => canExecute());
    }

    public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

    // Use async void to satisfy ICommand.Execute; exceptions will bubble (you can improve with try/catch/logging)
    public async void Execute(object parameter)
    {
        if (_executeAsync != null)
            await _executeAsync(parameter);
        else
            _executeSync?.Invoke(parameter);
    }

    public event EventHandler CanExecuteChanged;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}