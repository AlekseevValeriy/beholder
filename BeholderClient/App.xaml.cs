namespace Beholder
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
#if DEBUG
            return new Window(new AppShell());
#else
            return new Window(new AddressEnter());
#endif
        }
    }
}