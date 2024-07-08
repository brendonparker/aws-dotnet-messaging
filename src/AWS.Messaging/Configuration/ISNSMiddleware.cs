// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.\r
// SPDX-License-Identifier: Apache-2.0

using AWS.Messaging.Publishers.SNS;

namespace AWS.Messaging.Configuration;

/// <summary>
/// Used to manipulate the message or options prior to sending to the topic.
/// </summary>
public interface ISNSMiddleware
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="options"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<SNSOptions?> HandleAsync<T>(T message, SNSOptions? options);
}
