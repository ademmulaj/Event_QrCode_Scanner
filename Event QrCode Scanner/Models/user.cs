namespace Event_QrCode_Scanner.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string FirstLastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string QrCodeUrl { get; set; }

        public ICollection<QrCodeData> QrCodeScans { get; set; }
    }
}
