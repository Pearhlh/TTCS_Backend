using WebApplication1.Data;

namespace WebApplication1.Model
{

    public class FileModel
    {
        public Guid? Id { get; set; }
        public Guid AccessSourceId { get; set; }
        public ResourceTypeEnum ResourceType { get; set; }
        public required string FileType { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class FileDeleteModel
    {
        public required string ProductType { get; set; }
        public required string FileIdListString { get; set; }
    }
}
