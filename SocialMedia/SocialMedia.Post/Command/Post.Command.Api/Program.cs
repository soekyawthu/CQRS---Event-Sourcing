using Confluent.Kafka;
using MongoDB.Bson.Serialization;
using Post.Command.Api.Commands;
using Post.Command.Domain;
using Post.Command.Infrastructure.Config;
using Post.Command.Infrastructure.Dispatchers;
using Post.Command.Infrastructure.Handlers;
using Post.Command.Infrastructure.Producers;
using Post.Command.Infrastructure.Repositories;
using Post.Command.Infrastructure.Stores;
using Post.Common.Events;
using SocialMedia.Core.Events;
using SocialMedia.Core.Handlers;
using SocialMedia.Core.Infrastructure;
using SocialMedia.Core.Producers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

var serviceProvider = builder.Services.BuildServiceProvider();
var commandHandler = serviceProvider.GetRequiredService<ICommandHandler>();
var commandDispatcher = new CommandDispatcher();
commandDispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<EditPostCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
commandDispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => commandDispatcher);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<PostCreatedEvent>();
BsonClassMap.RegisterClassMap<PostEditedEvent>();
BsonClassMap.RegisterClassMap<PostRemovedEvent>();
BsonClassMap.RegisterClassMap<CommentAddedEvent>();
BsonClassMap.RegisterClassMap<CommentEditedEvent>();
BsonClassMap.RegisterClassMap<CommentRemovedEvent>();
BsonClassMap.RegisterClassMap<PostLikedEvent>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();