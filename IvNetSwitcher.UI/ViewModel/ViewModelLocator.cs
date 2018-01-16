using DryIoc;
using IvNetSwitcher.Core;

namespace IvNetSwitcher.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public static Container Container { get; set; } = Bootstrap.Container;

        public ViewModelLocator()
        {
            InitContainer();
        }

        public MainViewModel Main => Container.Resolve<MainViewModel>();

        private static void InitContainer()
        {
            Container.Register<MainViewModel>(Reuse.Singleton);
        }

        public static void Cleanup()
        {
            //
        }
    }
}