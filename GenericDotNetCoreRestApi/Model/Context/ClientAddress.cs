using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericDotNetCoreRestApi.Model.Context
{
    [Table(nameof(ClientAddress), Schema = "dbo")]
    public class ClientAddress
    {
        [Key]
        public int? ID { get; set; }
        public int? ClientId { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string Province { get; set; }
        public string Code { get; set; }
    }
}
