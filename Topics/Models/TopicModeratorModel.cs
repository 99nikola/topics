using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Topics.Models
{
    public class TopicModeratorModel
    {
        [Required(ErrorMessage = "This filed is required.")]
        public string ModUsername { get; set; }

        [Required]
        public string TopicName { get; set; }

        public List<string> Mods { get; set; }
    }
}