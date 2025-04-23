using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityWebSite.DatabaseModels
{

    [Table("Locations")]
    public class Location
    {

        [Key]
        public int LocationID { get; set; }

        [Required]
        public string LocationName { get; set; }

        private int? _ParentID { get; set; }
        public int? ParentID 
        { 
            get { return _ParentID; }
            set
            {
                if (value == 0)
                    value = null;
                _ParentID = value;
            } 
        }

        // Relationship
        public ICollection<Camera> Cameras { get; set; }

    }
}
