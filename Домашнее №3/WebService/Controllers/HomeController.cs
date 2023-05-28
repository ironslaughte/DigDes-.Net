using Microsoft.AspNetCore.Mvc;
using TextParser;

namespace WebService.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost("getWords")]
        [ProducesResponseType(200, Type = typeof(Dictionary<string, int>))]
        [ProducesResponseType(400)]
        public IActionResult UniqWords([FromBody]string content)
        {
            Dictionary<string ,int> dict = UniqWordsCounter.PublicGetAllUniqWordsInText(content);
            return Ok(dict);
        }
    }
}
