using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericDotNetCoreRestApi.Model.Context
{
    [Table(nameof(ClientContacts), Schema = "dbo")]
    public class ClientContacts
    {
        [Key]
        public int? ID { get; set; }
        public int? ClientId { get; set; }
        public string Type { get; set; }
        public string ContactNo { get; set; }
    }
}
