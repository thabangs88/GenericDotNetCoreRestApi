using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericDotNetCoreRestApi.Model.Context
{
    [Table(nameof(Client), Schema = "dbo")]
    public class Client
    {
        [Key]
        public int? ClientId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }
}
