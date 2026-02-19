using DPK.Mosavabat.Models.Abstractions.Queries;
using MediatR;

namespace DPK.Mosavabat.BusinessServices.Abstractions.Queries;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}