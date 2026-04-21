using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.BusinessLayer.Concrete;
using Carola.DataAccessLayer.Repositories.CategoryRepository;
using Carola.DtoLayer.CategoryDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class CategoryManager : GenericManager<ResultCategoryDto, Category>, ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly IMapper _mapper;

        public CategoryManager(ICategoryDal categoryDal, IMapper mapper) : base(categoryDal, mapper)
        {
            _categoryDal = categoryDal;
            _mapper = mapper;
        }

        public async Task TCreateCategoryAsync(CreateCategoryDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            await _categoryDal.InsertAsync(entity);
        }

        public async Task TUpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            await _categoryDal.UpdateAsync(entity);
        }

        public async Task<GetByIdCategoryDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _categoryDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdCategoryDto>(entity);
        }
    }
}