using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Attributes;

namespace WebApi.Controllers
{

    public class AppController : BaseController
    {
        // GET: api/App
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/App/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/App
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/App/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/App/5
        public void Delete(int id)
        {
        }
    }
}
