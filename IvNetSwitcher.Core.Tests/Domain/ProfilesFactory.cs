using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Kernel;
using IvNetSwitcher.Core.Abstractions;
using IvNetSwitcher.Core.Domain;
using IvNetSwitcher.Core.DomainServices;
using IvNetSwitcher.Core.Dto;
using Moq;

namespace IvNetSwitcher.Core.Tests.Domain
{

    public static class ProfilesFactory
    {
        private const string APP_GUID = "730DE481-2077-4D59-AF65-543669121202";

        private const string PWD =
            @"IbP8X/dRF+c3YeCDtBg4d7ZzhQvhYDIZirJ9gAt/eoXPgH3QOWGWpeG65ZfrzPb3d9K2sY17bojnsYck3gaWYD+F+vq4jrVqvrh0fei3l5gWkGiBjP0xNXGw7Nm3ds/Y";

        public static Profiles CreateProfiles()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(INetService),
                    typeof(FakeService)));

            var svc = new Mock<IUtilsService>().Object;
            svc.SetSalt(APP_GUID);

            return new Profiles(
                new Mock<INetService>().Object,
                new Mock<IUtilsService>().Object,
                new List<ProfileDto>
                {
                    fixture.Build<ProfileDto>().With(x => x.Id, 1).With(x => x.Active, false)
                        .With(x => x.Password, PWD).Create(),
                    fixture.Build<ProfileDto>().With(x => x.Id, 2).With(x => x.Active, true)
                        .With(x => x.Password, PWD).Create(),
                    fixture.Build<ProfileDto>().With(x => x.Id, 3).With(x => x.Active, false)
                        .With(x => x.Password, PWD).Create()
                });
        }
    }
}