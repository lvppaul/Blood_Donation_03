namespace BD.Repositories.Models.Entities
{
    public enum DonationStatus
    {
        Waiting = 0,        // Chờ
        Confirmed = 1,      // Đã xác nhận
        Donated = 2,        // Đã hiến
        Rejected = 3,       // Từ chối
        Canceled = 4        // Hủy bỏ
    }
}
