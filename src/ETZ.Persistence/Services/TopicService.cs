using ETZ.Application;
using ETZ.Application.DTOs.Topic;
using ETZ.Domain.Entities;
using ETZ.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ETZ.Persistence.Services;

public sealed class TopicService
{
    private readonly ETZDbContext _context;
    private readonly ILogger<TopicService> _logger;

    public TopicService(ETZDbContext context, ILogger<TopicService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Topic>> GetAllAsync()
        => await _context.Topics.AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.DisplayOrder).ToListAsync();

    public async Task<Topic?> GetByIdAsync(Guid id)
        => await _context.Topics.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    public async Task<List<TopicInformationDto>> GetTopicsByLanguage(LanguageCode lang)
    {
        return await _context.Topics
            .AsNoTracking()
            .Where(t => !t.IsDeleted && t.LanguageCode == lang)
            .OrderBy(t => t.DisplayOrder)
            .Select(t => new TopicInformationDto
            {
                Id = t.Id,
                TopicName = t.TopicName,
                DisplayOrder = t.DisplayOrder
            })
            .ToListAsync();
    }

    public async Task<Response> CreateAsync(TopicCreateUpdateDto dto)
    {
        var entity = new Topic
        {
            Id = Guid.NewGuid(),
            DisplayOrder = dto.DisplayOrder,
            TopicName = dto.TopicName,
            LanguageCode = dto.LanguageCode,
            CreationTime = DateTimeOffset.UtcNow,
            CreatorUserId = Constants.SystemGodUserId,
            IsDeleted = false,
            DeletionTime = null,
            DeleterUserId = null,
            LastModificationTime = null,
            LastModifierUserId = null
        };
        await _context.Topics.AddAsync(entity);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Topic created Id={Id}", entity.Id);
        return Response.Ok("Topic created successfully");
    }
    public async Task<Response> UpdateAsync(Guid id, TopicCreateUpdateDto dto)
    {
        if (dto == null) return Response.Fail("Payload is required");

        var entity = await _context.Topics.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) return Response.Fail("Topic not found");
        entity.TopicName = dto.TopicName;
        entity.DisplayOrder = dto.DisplayOrder;
        entity.LastModifierUserId = Constants.SystemGodUserId;
        entity.LastModificationTime = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Topic updated Id={Id}", id);
        return Response.Ok("Topic updated successfully");
    }
    public async Task<Response> DeleteAsync(Guid id)
    {
        var topic = _context.Topics.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (topic is null) return Response.Fail("Topic not found");
        topic.IsDeleted = true;
        topic.DeleterUserId = Constants.SystemGodUserId;
        topic.DeletionTime = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Topic deleted Id={Id}", id);
        return Response.Ok("Topic deleted successfully");
    }
        
}


