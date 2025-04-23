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
        public string StreamURL { get; set; }

        [Required]
        public string PublishURL { get; set; }


        // Foreign Key
        private int? _LocationID { get; set; }
        public int? LocationID { 
            get { return _LocationID; } 
            set { 
                if (value == 0) 
                    value = null; 
                _LocationID = value; 
            } 
        }

        public Location Location { get; set; }

    }
}
