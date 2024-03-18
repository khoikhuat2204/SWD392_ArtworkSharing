namespace DataAccessLayer.DTOs.RequestDTO
{
    public class UpdateSubRequestDTO
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public int SubId { get; set; }
        public string StripeToken { get; set; } = string.Empty;
        public bool IsYearly { get; set; }
    }
}
