﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IvNetSwitcher.UI.Shared
{
    public class MagicAttribute : Attribute { }

    public class NoMagicAttribute : Attribute { }

    [Magic]
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        protected virtual void RaisePropertyChanged([CallerMemberName] string property = "")
        {
            var e = PropertyChanged;
            e?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}