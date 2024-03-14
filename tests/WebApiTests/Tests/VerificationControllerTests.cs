using Moq;
using Xunit;
using System;
using System.IO;
using WebApi.Models;
using Domain.Models;
using Domain.Abstract;
using WebApi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Identity.Client.Extensions.Msal;

namespace WebApiTests.Tests
{
    public sealed class VerificationControllerTests
    {
        private readonly Mock<IFileStorage> fileStorageMock;
        private readonly Mock<IDocumentValidationStrategy> dtdValidatorMock;
        private readonly Mock<IDocumentValidationStrategy> schemaValidatorMock;

        public IFileStorage FileStorage 
        {
            get
            {
                return this.fileStorageMock.Object;
            }
        }

        public VerificationControllerTests()
        {
            this.fileStorageMock = new Mock<IFileStorage>();
            this.dtdValidatorMock = new Mock<IDocumentValidationStrategy>();
            this.schemaValidatorMock = new Mock<IDocumentValidationStrategy>();
        }

        [Theory]
        [InlineData("ectd-123456/0000/m1/tw/tw-regional")]
        [InlineData("ectd-123456/0000/m1/tw/tw-regional.pdf")]
        [InlineData("ectd-123456/0000/m1/tw/tw-regional.rtf")]
        public async Task Validate_WithoutXmlExtension_ProducesBadRequest(string fullPath)
        {
            var verificationController = this.CreateVerificationController();
            var response = await verificationController.Validate(this.CreateValidationRequestModel(fullPath));

            Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(400, (response.Result as ObjectResult)?.StatusCode);
        }

        [Theory]
        [InlineData("ectd-123456/0000/m1/tw/tw-regional.xml", true)]
        public async Task Validate_ValidModelState_DoesNotReturnAnyError(string fullPath, bool valid)
        {
            var verificationController = this.CreateVerificationController();
            await verificationController.Validate(this.CreateValidationRequestModel(fullPath));

            Assert.Equal(verificationController.ModelState.IsValid, valid);
        }

        [Theory]
        [InlineData(" ", false)]
        [InlineData(null, false)]
        public async Task Validate_InvalidValidModelState_ReturnsErrors(string fullPath, bool valid)
        {
            var verificationController = this.CreateVerificationController();
            verificationController.ModelState.TryAddModelError("key", "The DocumentFullPath field is required");
            
            await verificationController.Validate(this.CreateValidationRequestModel(fullPath));

            Assert.Equal(verificationController.ModelState.IsValid, valid);
        }

        [Theory]
        [InlineData("ectd-123456/0000/m1/ca/tw-regional.xl", false)]
        public async Task Validate_IncorrectFilePath_ProducesBadRequest(string fullPath, bool returnResult)
        {
            this.SetupStorageMethods(returnResult);

            var verificationController = this.CreateVerificationController();
            var response = await verificationController.Validate(this.CreateValidationRequestModel(fullPath));

            Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(400, (response.Result as ObjectResult)?.StatusCode);
        }

        [Theory]
        [InlineData("ectd-123456/0000/m1/tw/tw-regional.xml", false, true)]
        public async Task Validate_WithoutSuitableValidatorType_ProducesBadRequest(string filePath, bool canProcced, bool fileExists)
        {
            this.SetupStorageMethods(fileExists);
            this.SetupValidationResult(this.dtdValidatorMock, canProcced);
            this.SetupValidationResult(this.schemaValidatorMock, canProcced);

            var verificationController = this.CreateVerificationController();
            var response = await verificationController.Validate(this.CreateValidationRequestModel(filePath));

            Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal(400, (response.Result as ObjectResult)?.StatusCode);
        }

        [Theory]
        [InlineData("ectd-123456/0000/m1/tw/tw-regional.xml", true, true, false)]
        [InlineData("ectd-123456/0000/m1/tw/tw-regional.xml", true, false, true)]
        public async Task Validate_ValidatorsCanProcessFile_ReturnsNotNullValidationResult(string filePath, bool fileExists, bool canValidateDtd, bool canValidateSchema)
        {
            this.SetupStorageMethods(fileExists);
            this.SetupValidationResult(this.dtdValidatorMock, canValidateDtd);
            this.SetupValidationResult(this.schemaValidatorMock, canValidateSchema);

            var verificationController = this.CreateVerificationController();
            var response = await verificationController.Validate(this.CreateValidationRequestModel(filePath));

            var objectResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(objectResult.Value);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void VerificationController_HasNullArgumentInCtor_ThrowsArgumentNullException(bool isFirstParameterNull, bool isSecondParameterNull)
        {
            var secondParameter = isSecondParameterNull ? null : this.GetValidatorObjects();
            var firstParameter = isFirstParameterNull ? null : this.FileStorage;

            Assert.Throws<ArgumentNullException>(() => new ValidationController(firstParameter, secondParameter));
        }

        private void SetupStorageMethods(bool fileExists)
        {
            this.fileStorageMock.Setup(storage => storage.FindExistsAsync(It.IsAny<string>())).ReturnsAsync(fileExists);
            this.fileStorageMock.Setup(storage => storage.FindByFullPathAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<Stream>());
        }

        private void SetupValidationResult<TMock>(TMock validatorMock, bool value)
            where TMock : Mock<IDocumentValidationStrategy>
        {
            validatorMock.Setup(strategy => strategy.CanProcess(It.IsAny<Stream>())).Returns(value);
            validatorMock.Setup(strategy => strategy.ProcessAsync(It.IsAny<RequestModel>())).ReturnsAsync(new ValidationResult());
        }

        private ValidationController CreateVerificationController()
        {
            return new ValidationController(this.fileStorageMock.Object, this.GetValidatorObjects());
        }

        private ValidationRequestModel CreateValidationRequestModel(string fullPath)
        {
            return new ValidationRequestModel
            {
                DocumentFullPath = fullPath
            };
        }

        private IEnumerable<IDocumentValidationStrategy> GetValidatorObjects()
        {
            return new[] 
            { 
                this.dtdValidatorMock.Object, 
                this.schemaValidatorMock.Object 
            };
        }
    }
}