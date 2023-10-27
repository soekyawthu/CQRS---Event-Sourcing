using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;
using SocialMedia.Core.Consumers;
using EventHandler = Post.Query.Infrastructure.Handlers.EventHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Action<DbContextOptionsBuilder> configureDbContext =
    x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));

builder.Services.AddDbContext<PostDbContext>(configureDbContext);
builder.Services.AddSingleton<PostDbContextFactory>(new PostDbContextFactory(configureDbContext));
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IEventHandler, EventHandler>();
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

builder.Services.AddHostedService<ConsumerHostedService>();
//builder.Services.AddHostedService<ConsumerBackgroundService>();

var app = builder.Build();

//var dataContext = app.Services.GetRequiredService<PostDbContext>();
//dataContext.Database.EnsureCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();