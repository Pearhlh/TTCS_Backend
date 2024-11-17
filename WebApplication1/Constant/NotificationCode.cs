namespace TourishApi.Constant
{
    public class NotificationCode
    {
        public static readonly Dictionary<string, string> NOTIFI_CODE_VI = new Dictionary<string, string>
        {
            { "I411", " đã thêm tour: " },
            { "I412", " đã cập nhật tour: " },
            { "I413", " đã xóa tour: " },

            { "I421", "Thể loại tour thêm thành công" },
            { "I422", "Thể loại tour cập nhật thành công" },
            { "I423", "Thể loại tour xóa thành công" }
        };
    }
}
