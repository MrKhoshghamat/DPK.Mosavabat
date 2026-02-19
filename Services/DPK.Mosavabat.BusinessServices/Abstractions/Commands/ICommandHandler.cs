using DPK.Mosavabat.Models.Abstractions.Commands;
using MediatR;

namespace DPK.Mosavabat.BusinessServices.Abstractions.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResult> :
    IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
}