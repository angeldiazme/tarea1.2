namespace tarea1._2
{
    public partial class App : Application
    {
        public static Controllers.AutorController AutorController { get; private set; }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new vistas.MainPage());
            AutorController = new Controllers.AutorController();
        }
    }
}

