using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Features.Categories.Commands.CreateCategory
{
    /// <summary>
    /// Parameters to create a category
    /// </summary>
    public class CreateCategoryCommand : IRequest<int>
    {
        [SwaggerParameter(Description = "the category name")]
        public string Name { get; set; }
        
        [SwaggerParameter(Description = "the category description")]
        public string Description { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<int> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = mapper.Map<Category>(command);
            category = await categoryRepository.AddAsync(category);
            return category.Id;
        }
    }
}
