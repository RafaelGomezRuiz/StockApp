using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Features.Categories.Commands.DeleteCategory
{
    /// <summary>
    /// Parameters to delete a category
    /// </summary>
    public class DeleteCategoryByIdCommand : IRequest<int>
    {
        [SwaggerParameter(Description ="The id of the category to delete")]
        public int Id { get; set; }
    }

    public class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand,int>
    {
    
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public DeleteCategoryByIdCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<int> Handle(DeleteCategoryByIdCommand command, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(command.Id);
            if (category == null) throw new Exception("That category doesnt exists");
            await categoryRepository.DeleteAsync(category);
            return category.Id;
        }
    }
}
