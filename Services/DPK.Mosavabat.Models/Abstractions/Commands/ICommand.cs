using MediatR;

namespace DPK.Mosavabat.Models.Abstractions.Commands;

public interface ICommand<out TResult> : IRequest<TResult>
{
}

public interface ICommand : IRequest
{
}