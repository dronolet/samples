using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace apitest.Objects
{

    [DataContract]
    public class Company
    {

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "inn")]
        public string Inn { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }

    }
}
