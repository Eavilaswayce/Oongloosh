using Xamarin.Forms;

[assembly: ExportFont("Signika.ttf", Alias = "Signika")]
namespace Oongloosh_Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
