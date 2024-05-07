using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArcAndAll.Application.Behaviors
{
    public class FluentValidationBehavior<TRequest, TResponse> : 
        IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public IEnumerable<IValidator<TRequest>> validator { get; }

        public FluentValidationBehavior(IEnumerable<IValidator<TRequest>> validator)
        {
             this.validator = validator;
        }


        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = validator
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .GroupBy(x => x.ErrorMessage)
                .Where(f => f != null)
                .Select(x => x.First())
                . ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
