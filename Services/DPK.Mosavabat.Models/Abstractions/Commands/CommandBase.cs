namespace DPK.Mosavabat.Models.Abstractions.Commands;

public abstract class CommandBase
    : ICommand
{
}

public abstract class CommandBase<TResult>
    : ICommand<TResult>
{
}