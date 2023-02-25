using SocialNetworkBE.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {
    public class UserController : ApiController {

        private const string REFIX = "api/v1/user";

        [HttpPost]
        [Route(REFIX + "/")]
        public ResponseBase GetUserProfile() {
            return new ResponseBase();
        }
    }
}