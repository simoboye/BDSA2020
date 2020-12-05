using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDSA2020.Entities
{
    public class PlacementDescription : MatchingAttributes
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime LastApplyDate { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Uri Thumbnail { get; set; }

        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<PlacementDescriptionKeyword> Keywords { get; set; }

        public virtual ICollection<Saved> Students { get; set; }
    }
}
