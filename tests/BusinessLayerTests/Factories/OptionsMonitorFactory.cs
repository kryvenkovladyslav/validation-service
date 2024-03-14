using Moq;
using Microsoft.Extensions.Options;

namespace BusinessLayerTests.Factories
{
    public interface IOptionsMonitorFactory<TOptions>
    {
        public IOptionsMonitor<TOptions> Create(TOptions options);
    }

    public sealed class OptionsMonitorFactory<TOptions> : IOptionsMonitorFactory<TOptions>
        where TOptions : class
    {
        public IOptionsMonitor<TOptions> Create(TOptions options)
        {
            var optionsMonitor = new Mock<IOptionsMonitor<TOptions>>();
            optionsMonitor.Setup(monitor => monitor.CurrentValue).Returns(options);
            return optionsMonitor.Object;
        }
    }
}