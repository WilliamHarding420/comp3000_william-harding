using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityWebSite.DatabaseModels
{

    [Table("Cameras")]
    public class Camera
    {

        [Required]
        [Key]
        public int CameraID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string IP { get; set; }

        [Required]
        public string Port { get; set; }

        [Required]
        public string PublishURL { get; set; }

        public int LocationID { get; set; }

    }
}
