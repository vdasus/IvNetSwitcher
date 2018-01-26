using CSharpFunctionalExtensions;

namespace IvNetSwitcher.Core.Abstractions
{
    public interface IAppService
    {
        Result RegisterProfile();
        Result DeleteProfile();
        Result GoPlay();
        Result GoNext();
    }
}