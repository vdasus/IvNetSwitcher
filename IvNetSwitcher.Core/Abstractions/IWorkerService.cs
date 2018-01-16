﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IWorkerService
    {
        void Run(Profiles profiles, Uri hostToPing, int delay, int retry, int times = 0);
    }
}