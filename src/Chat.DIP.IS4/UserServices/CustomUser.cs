namespace Chat.DIP.IS4.UserServices
{
    public class CustomUser
    {
        public int Id { get; set; }
        public string SubjectId => Id.ToString();
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}