using CSharpFunctionalExtensions;

namespace IvNetSwitcher.Core.Domain
{
    public class Network: ValueObject<Network>
    {
        public int Id { get; }
        public string Name { get; }
        public uint SignalStrength { get; }
        public bool HasProfile { get; }
        public bool IsSecure { get; }
        public bool IsConnected { get; }

        public Network(int id, string name, uint signalStrength, bool hasProfile, bool isSecure, bool isConnected)
        {
            Id = id;
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
