using Xunit;
using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;

public class VirusRepositoryTests
{
    [Fact]
    public async Task CreateAsync_AddsVirus()
    {
        var mockSet = new Mock<DbSet<Virus>>();
        var mockContext = new Mock<ViroCureFal2024dbContext>();
        mockContext.Setup(m => m.Viruses).Returns(mockSet.Object);
        var repo = new VirusRepository(mockContext.Object);
        var virus = new Virus { VirusId = 1, VirusName = "Covid" };
        await repo.CreateAsync(virus);
        mockSet.Verify(m => m.AddAsync(virus, default), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }
} 