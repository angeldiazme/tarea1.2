using System.Collections.ObjectModel;

namespace tarea1._2.vistas;

public partial class nuevoAutor : ContentPage
{
    //Clase que permite guardar objetos que estan vinculados a un elemento de interface
    ObservableCollection<string> countries;

    Controllers.AutorController controller;
    FileResult photo; //Para tomar foto

    string nacionalidad;
    public nuevoAutor()
    {
        InitializeComponent();

        controller = new Controllers.AutorController();

        countries = new ObservableCollection<string>
            {
            "Honduras",
            "Nicaragua",
            "Panama",
            "Mexico",
            "España",
            "Italia"
        };

        countryPicker.ItemsSource = countries;
    }

    public nuevoAutor(Controllers.AutorController dbPath)
    {
        InitializeComponent();
        controller = dbPath;

        countries = new ObservableCollection<string>
            {
            "Honduras",
            "Nicaragua",
            "Panama",
            "Mexico",
            "España",
            "Italia"
            };

        countryPicker.ItemsSource = countries;
    }

    public string? GetImg64()
    {
        if (photo != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Stream stream = photo.OpenReadAsync().Result;
                stream.CopyTo(ms);
                byte[] data = ms.ToArray();

                String Base64 = Convert.ToBase64String(data);

                return Base64;
            }
        }
        return null;
    }

    public byte[]? GetImageArray()
    {
        if (photo != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Stream stream = photo.OpenReadAsync().Result;
                stream.CopyTo(ms);
                byte[] data = ms.ToArray();

                return data;
            }
        }
        return null;
    }

    private void countryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        nacionalidad = countryPicker.SelectedItem as string;
    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {

        string Nombres = txtNombres.Text;

        if (string.IsNullOrEmpty(Nombres))
        {
            await DisplayAlert("Advertencia", "Ingresar el nombre del autor", "ok");
            return;
        }
        else if (string.IsNullOrEmpty(nacionalidad))
        {
            await DisplayAlert("Advertencia", "Seleccione nacionalidad del autor", "ok");
            return;
        }

        var autor = new Models.Autor
        {
            Nombres = txtNombres.Text,
            Nacionalidad = nacionalidad,
            Foto = GetImg64()
        };

        try
        {
            if (controller != null)
            {
                if (await controller.storeAutor(autor) > 0)
                {
                    await DisplayAlert("Alerta", "Registro Ingresado", "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Advertencia", "Ocurrio un Error", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Advertencia", $"Error: {ex.Message}", "OK");
        }
    }

    private void btnRegresar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnfoto_Clicked(object sender, EventArgs e)
    {
        photo = await MediaPicker.CapturePhotoAsync();

        if (photo != null)
        {
            string photoPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using Stream sourcephoto = await photo.OpenReadAsync();
            using FileStream streamlocal = File.OpenWrite(photoPath);

            imgFoto.Source = ImageSource.FromStream(() => photo.OpenReadAsync().Result); //Para verla dentro de archivo

            await sourcephoto.CopyToAsync(streamlocal); //Para Guardarla local
        }
    }
}