using Moq;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Features.Chat;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;
using Serilog;
using Xunit;

namespace RealtimeChat.Tests.Unit.Features.Chat;

public class AddReactionCommandHandlerTests
{
    [Fact]
    public async Task Handle_RoomMessage_SendsToRoom()
    {
        var chatNotifier = new Mock<IChatNotifier>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var reactionRepo = new Mock<IMessageReactionRepository>();
        var messageRepo = new Mock<IMessageRepository>();

        var room = new ChatRoom { Id = 1, Name = "general" };
        var message = new Message { Id = 10, RoomId = 1, Room = room };

        messageRepo.Setup(m => m.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(message);

        unitOfWork.SetupGet(u => u.Messages).Returns(messageRepo.Object);
        unitOfWork.SetupGet(u => u.MessageReactions).Returns(reactionRepo.Object);
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new AddReactionCommandHandler(unitOfWork.Object, chatNotifier.Object, new LoggerConfiguration().CreateLogger());

        var payload = new ReactionDto { MessageId = 10, Emoji = "👍" };

        await handler.Handle(new AddReactionCommand("user-1", "User One", payload), CancellationToken.None);

        reactionRepo.Verify(r => r.AddAsync(It.Is<MessageReaction>(rx =>
            rx.MessageId == 10 && rx.UserId == "user-1" && rx.Emoji == "👍"), It.IsAny<CancellationToken>()), Times.Once);

        chatNotifier.Verify(n => n.SendToRoomAsync(
            "general",
            "MessageReactionAdded",
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
