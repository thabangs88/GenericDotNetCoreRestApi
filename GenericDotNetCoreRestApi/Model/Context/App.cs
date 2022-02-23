using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenericDotNetCoreRestApi.Model.Context
{
    [Table(nameof(App), Schema = "dbo")]
    public partial class App
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public bool Active { get; set; }
    }
}
