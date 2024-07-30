using Application.Repository;
using AutoMapper;
using StockApp.Core.Application.Interfaces.Services;

namespace Application.Services
{
    public class GenericService<SaveViewModel, ViewModel,Entity> : IGenericService<SaveViewModel,ViewModel,Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity> _repository;
        protected readonly IMapper _mapper;
        public GenericService(IGenericRepository<Entity> _repository, IMapper _mapper)
        {
            this._repository= _repository;
            this._mapper= _mapper;
        }

        public virtual async Task<SaveViewModel> Add(SaveViewModel vm)
        {
            Entity entity = _mapper.Map<Entity>(vm);
            entity = await _repository.AddAsync(entity);
            SaveViewModel saveViewModel = _mapper.Map<SaveViewModel>(entity);

            return saveViewModel;
        }

        public virtual async Task Update(SaveViewModel vm, int id)
        {
            Entity entity = _mapper.Map<Entity>(vm);
            await _repository.UpdateAsync(entity, id);
        }

        public virtual async Task Delete(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(entity);
        }

        public virtual async Task<SaveViewModel> GetByIdSaveViewModel(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);
            SaveViewModel vm = _mapper.Map<SaveViewModel>(entity);
            return vm;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModel()
        {
            var entityList = await _repository.GetAllAsync();
            return _mapper.Map<List<ViewModel>>(entityList);
            //return categorylist.Select(s => new ViewModel
            //{
            //    Name = s.Name,
            //    Description = s.Description,
            //    Id = s.Id,
            //}).ToList();
        }
    }
}
