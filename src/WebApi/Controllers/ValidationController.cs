using System;
using Domain.Models;
using WebApi.Models;
using Domain.Abstract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class ValidationController : ControllerBase
    {
        private readonly IFileStorage fileStorage;
        private readonly IEnumerable<IDocumentValidationStrategy> validators;

        public ValidationController(IFileStorage fileStorage, IEnumerable<IDocumentValidationStrategy> validators)
        {
            this.fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
            this.validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        [HttpPost]
        public async Task<ActionResult<ValidationResult>> Validate([FromBody] ValidationRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if(!await this.fileStorage.FindExistsAsync(model.DocumentFullPath)) 
            {
                return this.BadRequest("The blob does not exist");
            }

            using var stream = await this.fileStorage.FindByFullPathAsync(model.DocumentFullPath);

            foreach (var validator in this.validators)
            {
                if (validator.CanProcess(stream))
                {
                    var result = await validator.ProcessAsync(new RequestModel(model.DocumentFullPath, stream));
                    return this.Ok(result);
                }
            }

            return this.BadRequest(model);
        }
    }
}