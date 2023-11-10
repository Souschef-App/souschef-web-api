using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using souschef.server.Services;

namespace souschef.server.Controllers
{
    [ApiController]
    [Route("api/subtask")]
    public class SubTaskGenerationController : Controller
    {
        private readonly ISubTaskGenerationService m_SubTaskGenerationService;

        public SubTaskGenerationController(ISubTaskGenerationService _subTaskGenerationService)
        {
            m_SubTaskGenerationService = _subTaskGenerationService;
        }

        [HttpGet("session")]
        public ActionResult<IEnumerable<Recipe>> StartSubTaskBreakDownSession()
        {
            //create randome uuid
            m_SubTaskGenerationService.StartInferenceSession("my-id");

            //return uuid
            return Ok();
        }


        [HttpPost("request")]
        public async Task<ActionResult<IEnumerable<Recipe>>> RequestSubTaskGeneration([FromBody] BreakDownRequestDTO _dto)
        {
            if (_dto.Recipe == null)
                return new ContentResult() { Content = "Recipe can't be null", StatusCode = 500 };

            var subtask = await m_SubTaskGenerationService.RequestSubTaskGeneration(_dto.Recipe);
            if (subtask != null)
            {
                return Ok(subtask);
            }
            else
            {
                return new ContentResult() { Content = "Generation Failed", StatusCode = 500 };
            }
        }

        [HttpPost("request")]
        public async Task<ActionResult<IEnumerable<Recipe>>> RequestSubTaskRegeneration([FromBody] BreakDownRequestDTO _dto)
        {
            if (_dto.Recipe == null)
                return new ContentResult() { Content = "Recipe can't be null", StatusCode = 500 };

            var subtask = await m_SubTaskGenerationService.RequestRegenerationOfSubTask(_dto.Recipe, "00");
            if (subtask != null)
            {
                return Ok(subtask);
            }
            else
            {
                return new ContentResult() { Content = "Generation Failed", StatusCode = 500 };
            }
        }

        [HttpPost("request")]
        public async Task<ActionResult<IEnumerable<Recipe>>> RequestAllSubTaskRegeneration([FromBody] BreakDownRequestDTO _dto)
        {
            if (_dto.Recipe == null)
                return new ContentResult() { Content = "Recipe can't be null", StatusCode = 500 };

            var subtask = await m_SubTaskGenerationService.RequestRegenerationOfAllSubTask(_dto.Recipe, "01");
            if (subtask != null)
            {
                return Ok(subtask);
            }
            else
            {
                return new ContentResult() { Content = "Generation Failed", StatusCode = 500 };
            }
        }
    }
}
