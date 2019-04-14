//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//

using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing.SyncModel;

namespace FeelPhysics.HoloMagnet36
{
    /// <summary>
    /// Class that demonstrates a custom class using sync model attributes.
    /// </summary>
    [SyncDataClass]
    public class SyncSpawnedBarMagnetSyncPoint : SyncSpawnedObject
    {
        [SyncData]
        public SyncBool showsMagneticForceLines;
    }
}