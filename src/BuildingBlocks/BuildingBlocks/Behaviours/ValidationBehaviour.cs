using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> (IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest: ICommand
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failuers = validationResults.Where(r => r.Errors.Any())
                                            .SelectMany(r => r.Errors)
                                            .ToList();

            return await next();

        }
    }
}
