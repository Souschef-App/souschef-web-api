using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using souschef.server.Data.DTOs;
using souschef.server.Data.Models;
using souschef.server.Services.SubtaskGeneration;

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
        public async Task<ActionResult<IEnumerable<Data.Models.Task>>> RequestSubTaskGeneration([FromBody] BreakDownRequestDTO _dto)
        {
            if (_dto.Recipe == null)
                return new ContentResult() { Content = "Recipe can't be null", StatusCode = 500 };

            var subtasks = await m_SubTaskGenerationService.RequestSubTaskGeneration(_dto.Recipe);

            Debug.WriteLine(subtasks);
            if (subtasks != null)
            {
                return Ok(subtasks);
            }
            else
            {
                return new ContentResult() { Content = "Generation Failed", StatusCode = 500 };
            }
        }

        [HttpPost("retry-task")]
        public async Task<ActionResult<Data.Models.Task>> RequestSubTaskRegeneration([FromBody] RetryTaskDTO _dto)
        {
            Console.WriteLine("retry-task " + _dto);
            if (_dto.Prompt == null || _dto.Task == null)
                return new ContentResult() { Content = "Prompt or Task cannot be null", StatusCode = 402 };


            var subtask = await m_SubTaskGenerationService.RequestRegenerationOfSubTask(_dto.Prompt, _dto.Task);
            if (subtask != null)
            {
                return Ok(subtask);
            }
            else
            {
                return new ContentResult() { Content = "Generation Failed", StatusCode = 500 };
            }
        }

        [HttpPost("retry-all")]
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
