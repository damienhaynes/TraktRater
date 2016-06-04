namespace TraktRater.Sites.API.Flixster
{
    using System.Runtime.Serialization;

    [DataContract]
    public class FlixsterUser
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "firstName")]
        public string FirstName { get; set; }

        [DataMember(Name = "lastName")]
        public string LastName { get; set; }
    }
}
