using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.DbContextFile;
using WebApplication1.Model;
using WebApplication1.Model.VirtualModel;
using WebApplication1.Repository.Interface;

namespace WebApplication1.Repository.InheritanceRepo
{
    public class FileRepository : IFileRepository
    {
        private readonly MyDbContext _context;
        public FileRepository(MyDbContext _context)
        {
            this._context = _context;
        }

        public async Task<Response> Add(FileModel fileModel)
        {
            var file = new SaveFile
            {
                FileType = fileModel.FileType,
                ResourceType = fileModel.ResourceType,
                AccessSourceId = fileModel.AccessSourceId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Add(file);
            await _context.SaveChangesAsync();

            return new Response
            {
                resultCd = 0,
                MessageCode = "I912",
                returnId = file.Id
                // Create type success               
            };

        }

        public Response Delete(Guid id)
        {
            var file = _context.SaveFileList.FirstOrDefault((file
               => file.Id == id));
            var returnId = new Guid();

            if (file != null)
            {
                returnId = file.Id;
                _context.Remove(file);
                _context.SaveChanges();
            }

            return new Response
            {
                resultCd = 0,
                MessageCode = "I913",
                returnId = returnId
                // Delete type success               
            };
        }

        public async Task<Response> getById(Guid id)
        {
            var file = await _context.SaveFileList.AsSplitQuery().FirstOrDefaultAsync((file
                => file.Id == id));

            return new Response
            {
                resultCd = 0,
                Data = file,
                type = file.FileType
            };
        }

        public Response getByProductId(Guid id, ResourceTypeEnum type)
        {
            var fileList = _context.SaveFileList.Where((file
                => file.AccessSourceId == id && file.ResourceType == type)).OrderBy(entity => entity.CreatedDate).AsSplitQuery().ToList();

            return new Response
            {
                resultCd = 0,
                Data = fileList
            };
        }
    }
}
