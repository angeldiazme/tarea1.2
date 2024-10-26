namespace tarea1._2.vistas;

public partial class MainPage : ContentPage
{
	
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnVerAutores_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new verAutores());
        }

        private void btnAgregar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new nuevoAutor());
        }

        private void OnLinkTapped(object sender, EventArgs e)
        {
            if (sender is Label label && label.GestureRecognizers[0] is TapGestureRecognizer tapGestureRecognizer)
            {
                string url = tapGestureRecognizer.CommandParameter as string;
                if (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        Uri uri = new Uri(url);
                        Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                    }
                    catch (Exception ex)

                    {

                    }
                }
            }
        }
    }