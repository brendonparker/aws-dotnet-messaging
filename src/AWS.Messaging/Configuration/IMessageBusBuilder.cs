// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using AWS.Messaging.Publishers.EventBridge;
using AWS.Messaging.Publishers.SNS;
using AWS.Messaging.Publishers.SQS;
using AWS.Messaging.Serialization;
using AWS.Messaging.Services.Backoff;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.Messaging.Configuration;

/// <summary>
/// This builder interface is used to configure the AWS messaging framework, including adding publishers and subscribers.
/// </summary>
public interface IMessageBusBuilder
{
    /// <summary>
    /// Adds an SQS Publisher to the framework which will handle publishing
    /// the defined message type to the specified SQS queues URL.
    /// </summary>
    /// <param name="queueUrl">The SQS queue URL to publish the message to. If the queue URL is null, a message-specific queue
    /// URL must be specified on the <see cref="SQSOptions"/> when sending a message.</param>
    /// <param name="messageTypeIdentifier">The language-agnostic message type identifier. If not specified, the .NET type will be used.</param>
    IMessageBusBuilder AddSQSPublisher<TMessage>(string? queueUrl, string? messageTypeIdentifier = null);

    /// <summary>
    /// Adds an SNS Publisher to the framework which will handle publishing
    /// the defined message type to the specified SNS topic URL.
    /// </summary>
    /// <param name="topicUrl">The SNS topic URL to publish the message to. If the topic URL is null, a message-specific
    /// topic URL must be set on the <see cref="SNSOptions"/> when publishing a message.</param>
    /// <param name="messageTypeIdentifier">The language-agnostic message type identifier. If not specified, the .NET type will be used.</param>
    IMessageBusBuilder AddSNSPublisher<TMessage>(string? topicUrl, string? messageTypeIdentifier = null);

    /// <summary>
    /// Adds an EventBridge Publisher to the framework which will handle publishing the defined message type to the specified EventBridge event bus name.
    /// If you are specifying a global endpoint ID via <see cref="EventBridgePublishOptions"/>, then you must also include the <see href="https://www.nuget.org/packages/AWSSDK.Extensions.CrtIntegration">AWSSDK.Extensions.CrtIntegration</see> package in your application.
    /// </summary>
    /// <param name="eventBusName">The EventBridge event bus name or ARN where the message will be published. If the event bus name is null,
    /// a message-specific event bus must be set on the <see cref="EventBridgeOptions"/> when sending an event.</param>
    /// <param name="messageTypeIdentifier">The language-agnostic message type identifier. If not specified, the .NET type will be used.</param>
    /// <param name="options">Contains additional properties that can be set while configuring an EventBridge publisher</param>
    IMessageBusBuilder AddEventBridgePublisher<TMessage>(string? eventBusName, string? messageTypeIdentifier = null, EventBridgePublishOptions? options = null);

    /// <summary>
    /// Add a message handler for a given message type.
    /// The message handler contains the business logic of how to process a given message type.
    /// </summary>
    /// <param name="messageTypeIdentifier">The language-agnostic message type identifier. If not specified, the .NET type will be used.</param>
    IMessageBusBuilder AddMessageHandler<THandler, TMessage>(string? messageTypeIdentifier = null)
        where THandler : IMessageHandler<TMessage>;

    /// <summary>
    /// Adds an SQS queue to poll for messages.
    /// </summary>
    /// <param name="queueUrl">The SQS queue to poll for messages.</param>
    /// <param name="options">Optional configuration for polling message from SQS.</param>
    IMessageBusBuilder AddSQSPoller(string queueUrl, Action<SQSMessagePollerOptions>? options = null);

    /// <summary>
    /// Configures an instance of <see cref="SerializationOptions"/> to control the serialization/de-serialization logic for the application message.
    /// </summary>
    IMessageBusBuilder ConfigureSerializationOptions(Action<SerializationOptions> options);

    /// <summary>
    /// Adds a serialization callback that lets users inject their own metadata to incoming and outgoing messages.
    /// </summary>
    /// <param name="serializationCallback">An instance of <see cref="ISerializationCallback"/>that lets users inject their own metadata to incoming and outgoing messages.</param>
    IMessageBusBuilder AddSerializationCallback(ISerializationCallback serializationCallback);

    /// <summary>
    /// Adds a global message source to the message bus.
    /// This source will be added to the message envelope of all the messages sent through the framework.
    /// </summary>
    /// <param name="messageSource">The relative or absolute Uri to be used as a message source.</param>
    IMessageBusBuilder AddMessageSource(string messageSource);

    /// <summary>
    /// Appends a suffix to the message source that is either set using <see cref="AddMessageSource(string)"/>
    /// or computed by the framework in the absence of a user-defined source.
    /// </summary>
    /// <param name="suffix">The suffix to append to the message source.</param>
    IMessageBusBuilder AddMessageSourceSuffix(string suffix);

    /// <summary>
    /// Retrieve the Message Processing Framework section from <see cref="IConfiguration"/>
    /// and apply the Message bus configuration based on that section.
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    IMessageBusBuilder LoadConfigurationFromSettings(IConfiguration configuration);

    /// <summary>
    /// Add additional services to the <see cref="IMessageBusBuilder"/>. This method is used for AWS.Messaging plugins to add services for messaging.
    /// </summary>
    /// <param name="serviceDescriptor">The service descriptor for the added service.</param>
    /// <returns></returns>
    IMessageBusBuilder AddAdditionalService(ServiceDescriptor serviceDescriptor);

    /// <summary>
    /// Enables the visibility of data messages in the logging framework, exception handling and other areas.
    /// If this is enabled, messages sent by this framework will be visible in plain text across the framework's components.
    /// This means any sensitive user data sent by this framework will be visible in logs, any exceptions thrown and others.
    /// </summary>
    IMessageBusBuilder EnableMessageContentLogging();

    /// <summary>
    /// Configures the backoff policy used by <see cref="BackoffHandler"/> and its available options.
    /// </summary>
    IMessageBusBuilder ConfigureBackoffPolicy(Action<BackoffPolicyBuilder> configure);

    /// <summary>
    /// Configures middleware to be run against any send to the specified queue
    /// </summary>
    /// <param name="queueUrl"></param>
    /// <typeparam name="TMiddleware"></typeparam>
    /// <returns></returns>
    IMessageBusBuilder AddSQSMiddleware<TMiddleware>(string queueUrl) where TMiddleware : ISQSMiddleware;

    /// <summary>
    /// Configures middleware to be run against any send to the specified topic
    /// </summary>
    /// <param name="topicArn"></param>
    /// <typeparam name="TMiddleware"></typeparam>
    /// <returns></returns>
    IMessageBusBuilder AddSNSMiddleware<TMiddleware>(string topicArn) where TMiddleware : ISNSMiddleware;
}
