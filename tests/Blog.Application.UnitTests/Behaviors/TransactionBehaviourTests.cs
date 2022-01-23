using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Application.Behaviors;
using Blog.Application.Commands.CreatePost;
using Blog.Application.DTO;
using Blog.Application.Queries.GetPosts;
using Blog.Domain.DataAccess;
using MediatR;
using Moq;
using Xunit;

namespace Blog.Application.UnitTests.Behaviors;

public class TransactionBehaviourTests
{
    [Fact]
    public async Task When_QueryIsProvided_Should_NotStartTransaction()
    {
        // Arrange
        var uowMock = new Mock<IUnitOfWork>();
        var sut = new TransactionBehaviour<GetPostsQuery,PostDTO[]>(uowMock.Object);
        
        // Act
        await sut.Handle(new GetPostsQuery(), CancellationToken.None, () => Task.FromResult(Array.Empty<PostDTO>()));
        
        // Assert
        uowMock.Verify(x => x.BeginTransactionAsync(), Times.Never);
    }
    
    [Fact]
    public async Task When_CommandIsProvided_Should_StartsTransaction()
    {
        // Arrange
        var uowMock = new Mock<IUnitOfWork>();
        var sut = new TransactionBehaviour<CreatePost, Unit>(uowMock.Object);
        
        // Act
        await sut.Handle(new CreatePost(), CancellationToken.None, () => Unit.Task);
        
        // Assert
        uowMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
    }
    
    [Fact]
    public async Task When_CommandIsProvided_Should_ClosesTransaction()
    {
        // Arrange
        var uowMock = new Mock<IUnitOfWork>();
        var sut = new TransactionBehaviour<CreatePost, Unit>(uowMock.Object);
        
        // Act
        await sut.Handle(new CreatePost(), CancellationToken.None, () => Unit.Task);
        
        // Assert
        uowMock.Verify(x => x.CommitTransactionAsync(It.IsAny<ITransaction>()), Times.Once);
    }
    
    [Fact]
    public async Task When_UnitOfWorkGotActiveTransaction_Should_NotStartTransaction()
    {
        // Arrange
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(x => x.HasActiveTransaction).Returns(true);
        var sut = new TransactionBehaviour<CreatePost, Unit>(uowMock.Object);
        
        // Act
        await sut.Handle(new CreatePost(), CancellationToken.None, () => Unit.Task);
        
        // Assert
        uowMock.Verify(x => x.BeginTransactionAsync(), Times.Never);
    }
    
    [Fact]
    public async Task When_UnitOfWorkGotActiveTransaction_Should_NotCommitTransaction()
    {
        // Arrange
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(x => x.HasActiveTransaction).Returns(true);
        var sut = new TransactionBehaviour<CreatePost, Unit>(uowMock.Object);
        
        // Act
        await sut.Handle(new CreatePost(), CancellationToken.None, () => Unit.Task);
        
        // Assert
        uowMock.Verify(x => x.CommitTransactionAsync(It.IsAny<ITransaction>()), Times.Never);
    }
}