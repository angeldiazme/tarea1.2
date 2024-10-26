using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace tarea1._2.vistas;

public partial class actuAutor : ContentPage 
{
    private int autorId;
    ObservableCollection<string> countries;

    Controllers.AutorController controller;
    List<Models.Autor> autores;
    FileResult photo;

    string nacionalidad;

    public actuAutor(int authorId)
    {
        InitializeComponent();
        this.autorId = authorId;

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
        BuscarAutor(authorId);
    }
    public actuAutor(Controllers.AutorController dbPath)
    {
        InitializeComponent();
        controller = dbPath;

        countries = new ObservableCollection<string>
        {
            
        };

        countryPicker.ItemsSource = countries;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        autores = await controller.getListAutor();

    }

    private async void BuscarAutor(int authorId)
    {
        autores = await controller.getListAutor();

        var results = autores
            .Where(author => author.Id == authorId)
            .ToList();



        if (results.Any())
        {
            var author = results.First();

            imgFoto.Source = author.Foto;
            txtNombres.Text = author.Nombres;
            countryPicker.SelectedItem = author.Nacionalidad;
        }
        else
        {
            await DisplayAlert("Error", "Id no encontrado", "OK");
        }
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
            await DisplayAlert("Mensaje","Porfavor Ingresar el nombre del autor", "ok");
            return;
        }
        else if (string.IsNullOrEmpty(nacionalidad))
        {
            await DisplayAlert("Mensaje", "Porfavor seleccione la nacionalidad del autor", "ok");
            return;
        }

        var autor = new Models.Autor
        {
            Id = autorId,
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
                    await DisplayAlert("Mensaje", "Registro Actualizado", "ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Mensaje", "Ocurrio un Error", "ok");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrio un Error: {ex.Message}", "OK");
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