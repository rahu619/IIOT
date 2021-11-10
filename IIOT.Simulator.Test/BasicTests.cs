using IIOT.Simulator.Configuration;
using IIOT.Simulator.Messaging;
using IIOT.Simulator.Messaging.Send;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IIOT.Simulator.Test;
[TestClass]
public class BasicTests
{
    [TestMethod]
    public void Invoking_Message_Received_Successfully()
    {
        var stubSensor = new Mock<ISensor>();
        stubSensor.Setup(x=>x.Generate().GetAwaiter().GetResult()).Raises(x => x.MessageReceived += null, this);

        var stubLogger = Mock.Of<ILogger<MessageSender>>();
        var stubConfiguration = new Mock<IOptions<MessageSenderConfiguration>>();
        stubConfiguration.Setup(x => x.Value).Returns(new MessageSenderConfiguration { });
        var messageSenderObj = new MessageSender(stubLogger, stubConfiguration.Object, stubSensor.Object);

        var mockMessageSender = new Mock<MessageSender>(stubLogger, stubConfiguration.Object, stubSensor.Object);
        mockMessageSender.Verify(x => x.SensorFailureEvent(It.IsAny<object>()), Times.Never);

    }
}