
using System.Windows.Input;

namespace WpfApplication.Helpers;
internal class LambdaCommand : Command
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    private void ExecSquare(int a, int b, int c, int d) => Console.WriteLine(a*b*c*d);

    public LambdaCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
    {
        _execute = Execute ?? throw new ArgumentNullException(nameof(Execute)); //проверка на введенность значения
        _canExecute = CanExecute;
    }
    public LambdaCommand(Func<Task> Execute, bool CanExecute = true) : this(
           async _ => await Execute(),
           _ => CanExecute)
    { }
    public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true; //если не передана команда, то все равно true
    public override void Execute(object parameter) => _execute(parameter);

}

internal abstract class Command : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public abstract bool CanExecute(object parameter);
    public abstract void Execute(object parameter);

}