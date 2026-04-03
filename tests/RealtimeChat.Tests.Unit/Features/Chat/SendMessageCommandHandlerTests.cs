using FluentAssertions;
using Moq;
using RealtimeChat.Application.DTOs;
using RealtimeChat.Application.Features.Chat;
using RealtimeChat.Application.Interfaces;
using RealtimeChat.Domain.Entities;
using RealtimeChat.Domain.Interfaces;
using Serilog;
using Xunit;

namespace RealtimeChat.Tests.Unit.Features.Chat;

public class SendMessageCommandHandlerTests
{
    [Fact]
    public async Task Handle_RoomMessage_PersistsAndBroadcasts()
    {
        var chatNotifier = new Mock<IChatNotifier>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var messageRepository = new Mock<IMessageRepository>();
        var roomRepository = new Mock<IChatRoomRepository>();

        var room = new ChatRoom { Id = 5, Name = "general" };
        roomRepository.Setup(r => r.GetByNameAsync("general", It.IsAny<CancellationToken>()))
            .ReturnsAsync(room);
        roomRepository.Setup(r => r.IsMemberAsync(room.Id, "user-1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        unitOfWork.SetupGet(u => u.Messages).Returns(messageRepository.Object);
        unitOfWork.SetupGet(u => u.ChatRooms).Returns(roomRepository.Object);
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new SendMessageCommandHandler(chatNotifier.Object, unitOfWork.Object, new LoggerConfiguration().CreateLogger());

        var payload = new SendMessageDto
        {
            Content = "Hello room",
            RoomName = "general"
        };

        await handler.Handle(new SendMessageCommand("user-1", "User One", payload), CancellationToken.None);

        messageRepository.Verify(m => m.AddAsync(It.Is<Message>(msg =>
            msg.Content == "Hello room" && msg.RoomId == room.Id && msg.SenderId == "user-1"), It.IsAny<CancellationToken>()), Times.Once);

        chatNotifier.Verify(n => n.SendToRoomAsync(
            "general",
            "ReceiveRoomMessage",
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DirectMessage_PersistsAndSendsToUsers()
    {
        var chatNotifier = new Mock<IChatNotifier>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var messageRepository = new Mock<IMessageRepository>();
        var roomRepository = new Mock<IChatRoomRepository>();

        unitOfWork.SetupGet(u => u.Messages).Returns(messageRepository.Object);
        unitOfWork.SetupGet(u => u.ChatRooms).Returns(roomRepository.Object);
        unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new SendMessageCommandHandler(chatNotifier.Object, unitOfWork.Object, new LoggerConfiguration().CreateLogger());

        var payload = new SendMessageDto
        {
            Content = "Hello direct",
            ReceiverId = "user-2"
        };

        await handler.Handle(new SendMessageCommand("user-1", "User One", payload), CancellationToken.None);

        messageRepository.Verify(m => m.AddAsync(It.Is<Message>(msg =>
            msg.Content == "Hello direct" && msg.ReceiverId == "user-2" && msg.SenderId == "user-1"), It.IsAny<CancellationToken>()), Times.Once);

        chatNotifier.Verify(n => n.SendToUserAsync(
            "user-2",
            "ReceiveMessage",
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()), Times.Once);

        chatNotifier.Verify(n => n.SendToUserAsync(
            "user-1",
            "ReceiveMessage",
            It.IsAny<object?[]>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
