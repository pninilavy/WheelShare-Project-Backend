namespace WheelShare.Models
{
    public class TokenAndName
    {
        public string Token {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public TokenAndName(string token,string firstName,string lastName)
        {
            this.Token = token;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
