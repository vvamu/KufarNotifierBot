namespace ConsoleApplication.Commands.Base;

public interface ICommand
{
    string Name { get; }
    string[] Aliases { get; }
    string Description { get; }
    Task<int> ExecuteAsync(string[] args, CancellationToken ct = default);
}
