using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProGaudi.Tarantool.Client;

namespace Service.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TarantoolConnectionController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Check(string connectionstring)
        {
            try
            {
                var q = new Box(new ProGaudi.Tarantool.Client.Model.ClientOptions(connectionstring));
                await q.Connect();
                return "ok";
            }
            catch (Exception ex)
            {
                return "Error! " + ex.Message;
            }
        }
    }
}
