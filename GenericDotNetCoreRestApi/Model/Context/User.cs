using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericDotNetCoreRestApi.Model.Context
{
    [Table(nameof(User), Schema = "dbo")]
    public partial class User
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }   
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public int CompanyID { get; set; }
        public int AppID { get; set; }
        public bool Active { get; set; }
    }
}
