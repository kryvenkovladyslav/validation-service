using System;
using Domain.Models;
using WebApi.Models;
using Domain.Abstract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public sealed class ValidationController : ControllerBase
    {
        private readonly IDocumentStorage documentStorage;

        private readonly IXmlValidationManager validationManager;

        public ValidationController(IDocumentStorage documentStorage, IXmlValidationManager validationManager)
        {
            this.documentStorage = documentStorage ?? throw new ArgumentNullException(nameof(documentStorage));
            this.validationManager = validationManager ?? throw new ArgumentNullException(nameof(validationManager));
        }

        [HttpPost]
        public async Task<ActionResult<ValidationResult>> Validate([FromBody] ValidationRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            using var documentStream = await this.documentStorage.FindByFullPathAsync(model.DocumentFullPath);
            var validationResult = await this.validationManager.ValidateDocumentAsync(model.DocumentFullPath, documentStream);

            return this.Ok(validationResult);
        }
    }
}