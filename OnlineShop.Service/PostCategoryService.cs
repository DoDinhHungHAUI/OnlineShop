using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Repositories;
using OnlineShop.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Service
{

    public interface IPostCategoryService
    {
        void Add(PostCategories postCategory);
        void Update(PostCategories postCategory);
        void Delete(int id);
        IEnumerable<PostCategories> GetAll();
        IEnumerable<PostCategories> GetAllByParentId(int parentId);
        PostCategories getById(int id);
    }

    public class PostCategoryService : IPostCategoryService
    {
        IPostCategoryRepository _postCategoryRepository;
        IUnitOfWork _unitOfWork;
        public PostCategoryService(IPostCategoryRepository postCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._postCategoryRepository = postCategoryRepository;
            this._unitOfWork = unitOfWork;
        }
        public void Add(PostCategories postCategory)
        {
            _postCategoryRepository.Add(postCategory);
        }

        public void Delete(int id)
        {
            _postCategoryRepository.Delete(id);
        }

        public IEnumerable<PostCategories> GetAll()
        {
            return _postCategoryRepository.GetAll();
        }

        public IEnumerable<PostCategories> GetAllByParentId(int parentId)
        {
            return _postCategoryRepository.GetMulti(x => x.Status && x.ParentID == parentId);
        }

        public PostCategories getById(int id)
        {
            return _postCategoryRepository.GetSingleById(id);
        }

        public void Update(PostCategories postCategory)
        {
            _postCategoryRepository.Update(postCategory);
        }
    }
}
