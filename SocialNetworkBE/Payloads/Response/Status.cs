using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payload.Response {
    public enum Status {
        Success, Failure, Error, WrongFormat, Fobident, Unauthorize
    }
}