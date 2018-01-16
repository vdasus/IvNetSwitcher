using System.Reflection;
using AutoFixture.Kernel;

namespace IvNetSwitcher.Core.Tests.SharedForTests
{
    public partial class ProfileTests
    {
        public class PasswordArg<T> : ISpecimenBuilder where T : class
        {
            private readonly string _value;

            public PasswordArg(string value)
            {
                _value = value;
            }

            public object Create(object request, ISpecimenContext context)
            {
                if (!(request is ParameterInfo pi))
                    return new NoSpecimen();

                if (pi.Member.DeclaringType != typeof(T) ||
                    pi.ParameterType != typeof(string) ||
                    pi.Name.ToLower() != "password")
                    return new NoSpecimen();

                return _value;
            }
        }
    }
}