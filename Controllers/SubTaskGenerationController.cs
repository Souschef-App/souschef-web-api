using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.Models;
using souschef.server.Services;

namespace souschef.server.Controllers
{
    public class SubTaskGenerationController : Controller
    {
        private readonly ISubTaskGenerationService m_SubTaskGenerationService;

        public SubTaskGenerationController(ISubTaskGenerationService _subTaskGenerationService)
        {
            m_SubTaskGenerationService = _subTaskGenerationService;
        }

        [HttpGet("subtask-session")]
        public ActionResult<IEnumerable<Recipe>> StartSubTaskBreakDownSession()
        {
            m_SubTaskGenerationService.StartInferenceSession("my-id");

            return Ok();
        }


        [HttpPost("subtask")]
        public ActionResult<IEnumerable<Recipe>> RequestSubTaskGeneration()
        {
            var subtask = m_SubTaskGenerationService.RequestSubTaskGeneration("my-id", "fry the banannas");
            if (subtask != null)
            {
                return Ok("hey");
            }
            else
            {
                return new ContentResult() { Content = "Generation Failed", StatusCode = 500 };
            }
        }
    }
}
