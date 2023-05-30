// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace VisualTestUtils.AppConnector.Messages;

public enum AppConnectorMessageType : ushort
{
    TestRequest = 1,
    TestResponse = 2,
    ClientRegistration = 3,
    OnScreenshotRequest = 4,
    OnScreenshotResponse = 5,
    AppClientDiscoveryResponse = 6,
    AppClientDisconnect = 7,
    AppClientConnect = 8,
}
