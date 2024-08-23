using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Features.Categories.Commands.UpdateCategory
{
    /// <summary>
    /// Parameters to update a category
    /// </summary>
    public class UpdateCategoryCommand : IRequest<UpdateCategoryResponse>
    {
     
        [SwaggerParameter(Description ="category id to update")]
        public int Id { get; set; }
        [SwaggerParameter(Description = "category name to update")]
        public string Name { get; set; }
        [SwaggerParameter(Description ="category description to update")]
        public string Description { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<UpdateCategoryResponse> Handle (UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(command.Id);
            if (category == null)
            {
                throw new Exception("That category doesnt exists");
            }
            else
            {
                category = mapper.Map<Category>(command);
                await categoryRepository.UpdateAsync(category, category.Id);
                var categoryResponse = mapper.Map<UpdateCategoryResponse>(category);
                return categoryResponse;
            }

        }
    }

}
