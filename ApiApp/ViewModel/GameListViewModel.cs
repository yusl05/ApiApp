using RawGames.Model;
using RawGames.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RawGames.ViewModel
{
    public class GameListViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private bool _isLoadingMore = false;
        private string _searchText = string.Empty;

        public ObservableCollection<Game> Games { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    // Cuando cambia el texto, lanzamos búsqueda
                    Task.Run(async () => await LoadGamesAsync());
                }
            }
        }

        public bool IsLoadingMore
        {
            get => _isLoadingMore;
            set => SetProperty(ref _isLoadingMore, value);
        }

        public ICommand LoadGamesCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand GameSelectedCommand { get; }

        public GameListViewModel(ApiService apiService)
        {
            _apiService = apiService;
            Title = "Álbumes de Música";

            LoadGamesCommand = new Command(async () => await LoadGamesAsync());
            RefreshCommand = new Command(async () => await RefreshGameAsync());
            GameSelectedCommand = new Command<Game>(OnGameSelected);

            // Cargar álbumes al inicializar
            Task.Run(async () => await LoadGamesAsync());
        }

        private void OnGameSelected(Game game)
        {
            if (game == null)
                return;

            Application.Current?.MainPage?.DisplayAlert("Álbum Seleccionado", $"{game.Name} - {game.Artist}", "OK");
        }

        private async Task LoadGamesAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Games.Clear();

                Response? response;

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    // Si no hay búsqueda, puedes traer un artista por defecto
                    response = await _apiService.SearchAlbumsAsync("Coldplay");
                }
                else
                {
                    response = await _apiService.SearchAlbumsAsync(SearchText);
                }

                if (response?.Results != null)
                {
                    foreach (var album in response.Results)
                    {
                        Games.Add(album);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar álbumes: {ex.Message}");
                await Application.Current!.MainPage!.DisplayAlert("Error", "No se pudieron cargar los álbumes. Verifica la conexión a Internet.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshGameAsync()
        {
            await LoadGamesAsync();
        }
    }
}