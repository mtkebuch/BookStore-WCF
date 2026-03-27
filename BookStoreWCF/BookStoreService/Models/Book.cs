using System.Runtime.Serialization;

namespace BookStoreService.Models
{
    [DataContract]
    public class Book
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string ISBN { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int Stock { get; set; }

        [DataMember]
        public string PublishedDate { get; set; }
    }
}