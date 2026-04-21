using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.VideoRepository
{
    public class EfVideoDal : GenericRepository<Video>, IVideoDal
    {
        public EfVideoDal(CarolaContext context) : base(context) { }
    }
}