using CSharpFunctionalExtensions;

namespace IvNetSwitcher.Core.Domain
{
    public class Network: ValueObject<Network>
    {
        public string Name { get; }
        public uint SignalStrength { get; }
        public bool HasProfile { get; }
        public bool IsSecure { get; }
        public bool IsConnected { get; }

        public Network(string name, uint signalStrength, bool hasProfile, bool isSecure, bool isConnected)
        {
            Name = name;
            SignalStrength = signalStrength;
            HasProfile = hasProfile;
            IsSecure = isSecure;
            IsConnected = isConnected;
        }

        #region Overrides of ValueObject<Network>

        protected override bool EqualsCore(Network other)
        {
            return Name == other.Name;
        }

        protected override int GetHashCodeCore()
        {
            return Name.GetHashCode();
        }

        #endregion
    }
}
