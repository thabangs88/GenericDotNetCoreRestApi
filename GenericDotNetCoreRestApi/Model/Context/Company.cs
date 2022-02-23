using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericDotNetCoreRestApi.Model.Context
{
    [Table(nameof(Company), Schema = "dbo")]
    public partial class Company
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
