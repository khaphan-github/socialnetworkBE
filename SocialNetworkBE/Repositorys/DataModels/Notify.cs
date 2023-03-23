using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Repositorys.DataModels {
    public class Notify {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }

    }
}