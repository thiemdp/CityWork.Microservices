using CityWork.Application;

namespace CityWork.Services.Product.API
{
    public class DeleteProductCommand:Entity, IRequest
    {

    }
    public class DeleteProductCommandHander : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly ICRUDServices<Product> _crudServices;

        public DeleteProductCommandHander(ICRUDServices<Product> crudServices)
        {
            _crudServices = crudServices;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
           await _crudServices.DeleteByIdAsync(request.Id,cancellationToken);
            return Unit.Value;
        }
    }
}
