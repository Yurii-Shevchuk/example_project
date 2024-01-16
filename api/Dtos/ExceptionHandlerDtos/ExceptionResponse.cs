using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace api.Dtos.ExceptionHandlerDtos
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);

}