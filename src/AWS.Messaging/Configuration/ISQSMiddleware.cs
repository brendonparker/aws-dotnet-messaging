// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.\r
// SPDX-License-Identifier: Apache-2.0

using AWS.Messaging.Publishers.SQS;

namespace AWS.Messaging.Configuration;

/// <summary>
/// Used to manipulate the message or options prior to sending to the queue.
/// </summary>
public interface ISQSMiddleware
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="options"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<SQSOptions?> HandleAsync<T>(T message, SQSOptions? options);
}
