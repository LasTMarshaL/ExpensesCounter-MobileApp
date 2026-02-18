using Android.App;
using Android.Runtime;

namespace ExpensesCounterMobileApplication // Conect class to the main project namespace
{
    [Application]
    public class MainApplication : MauiApplication   
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
