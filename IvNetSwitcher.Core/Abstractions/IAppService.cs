﻿using System;
using CSharpFunctionalExtensions;
using IvNetSwitcher.Core.Domain;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IAppService
    {
        Profiles LoadData(Profiles profiles);

        Result RegisterProfile();
        Result EditProfile();
        Result DeleteProfile();

        Result GoPlay();
        Result GoNext();

        void Run(Uri hostToPing, int delay, int retry, int times = 0);
    }
}