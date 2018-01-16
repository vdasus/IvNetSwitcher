using System.Reflection;
using AutoFixture.Kernel;

namespace IvNetSwitcher.Core.Tests.SharedForTests
{
    public partial class ProfileTests
    {
        public class SaltArg<T> : ISpecimenBuilder where T : class
        {
            private readonly string _value;

            public SaltArg(string value)
            {
                _value = value;
            }

            public object Create(object request, ISpecimenContext context)
            {
                if (!(request is ParameterInfo pi))
                    return new NoSpecimen();

                if (pi.Member.DeclaringType != typeof(T) ||
                    pi.ParameterType != typeof(string) ||
                    pi.Name != "salt")
                    return new NoSpecimen();

                return _value;
            }
        }
    }
}