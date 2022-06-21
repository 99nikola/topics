using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topics.Models
{
    public class ModalPostModel
    {

        public PostModel Post { get; set; }
        public string Id { get; set; }
        public string Label { get; set; }
        public ActionType Action { get; set; }
    }
}