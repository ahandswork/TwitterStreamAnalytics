﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tweetinvi.Core.Extensions;
using TwitterStreamAnalytics.Domain.Entities;
using TwitterStreamAnalytics.Domain.Events;
using TwitterStreamAnalytics.Infrastructure.Persistence;

namespace TwitterStreamAnalytics.Application.EventConsumers;

public class TweetReceivedConsumer : IConsumer<ITweetReceived>
{
    private readonly AnalyticsContext _dbContext;
    private readonly ILogger<TweetReceivedConsumer> _logger;

    public TweetReceivedConsumer(AnalyticsContext dbContext, ILogger<TweetReceivedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ITweetReceived> context)
    {
        AddTweet(context.Message);
        await AddHashtags(context.Message);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Tweet added: {Id}", context.Message.Id);
    }

    private void AddTweet(ITweetReceived tweet)
    {
        _dbContext.Tweets.Add(new Tweet(tweet.Id));
    }

    private async Task AddHashtags(ITweetReceived tweet)
    {
        if (!tweet.Hashtags.IsNullOrEmpty())
        {
            // TODO: handle concurrency conflicts on IncrementCount()
            var existingHashtags = await _dbContext.Hashtags
                .Where(h => tweet.Hashtags.Contains(h.Tag))
                .ToListAsync();
            foreach (var hashtag in existingHashtags) hashtag.IncrementCount();

            // TODO: handle concurrency conflicts on AddRange()
            var newHashtags = tweet.Hashtags.ExceptBy(existingHashtags.Select(h => h.Tag), h => h);
            _dbContext.Hashtags.AddRange(newHashtags.Select(h => new Hashtag(h)));
        }
    }
}