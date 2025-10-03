using kogui.Models;
using kogui.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace kogui
{
    public partial class MainPage : ContentPage
    {
        private readonly CorService _corService;
        public ObservableCollection<ChaveCor> Chaves { get; set; } = new();

        private readonly string[] hexordem = new[]
        {
            "#0000FF", // blue
            "#00FF00", // green
            "#FFFFFF", // white
            "#FF0000", // red
            "#FFA500", // orange
            "#FFFF00", // yellow
            "#000000" // black 
        };

        public MainPage(CorService corService)
        {
            InitializeComponent();

            _corService = corService;

            //criando lista com ObservableCollection para uma lista dinamica
            Chaves = new ObservableCollection<ChaveCor>()  //REQUISITO 1
            {
                new ChaveCor("magentaFunchsia", ""),    //aspas fechado representa vazio
                new ChaveCor("white", "para"),
                new ChaveCor("blue", "pares"),
                new ChaveCor("green", "alterar"),
                new ChaveCor("black", "#"),
                new ChaveCor("weborange", "e"),
                new ChaveCor("yellow", "impares"),
                new ChaveCor("red", "\" \""),           //tratamento para o aspas com espaço no meio(minha interpretação)
                new ChaveCor("coconut", "Busca"),
                new ChaveCor("cyanaqua", "primos")
            };

            ChavesCollection.ItemsSource = Chaves; // ChavesCollection collection view do xaml
        }

        // handler do botão da seção 2
        private async void OnFetchColorsClicked(object sender, EventArgs e)
        {
            await FetchNamesAndBuildPhraseAsync();
        }

        private async Task FetchNamesAndBuildPhraseAsync()
        {
            FraseLabel.Text = "Processando...";
            ColorCards.ItemsSource = null;

            var cards = new System.Collections.Generic.List<(string Name, string Hex)>();
            var componentes = new System.Collections.Generic.List<string>();

            foreach (var hex in hexordem)
            {
                var name = await _corService.GetColorNameFromHexAsync(hex) ?? hex; // fallback para hex se API falhar
                cards.Add((Name: name, Hex: hex)); // mapear o nome retornado para nossa lista de ChaveCor (normalização + aliases)

                var matched = FindComponenteByApiName(name);
                componentes.Add(matched ?? "");
            }

            // mostrar cards com Tolist
            ColorCards.ItemsSource = cards.Select(c => new { c.Name, c.Hex }).ToList();

            // construir a frase
            var phrase = BuildPhraseFromComponents(componentes);
            FraseLabel.Text = phrase;

        }

        // função que mapeia nome vindo da API para o Componente da sua ChaveCor
        private string FindComponenteByApiName(string apiName)
        {
            if (string.IsNullOrEmpty(apiName))
                return null;

            var normalized = apiName.ToLowerInvariant().Replace(" ", "").Replace("-", ""); // aliases conhecidos entre a saída da API e os nomes usados na sua lista
            var alias = new System.Collections.Generic.Dictionary<string, string>
            {
                { "orange", "weborange" },
                { "weborange", "weborange" },
                { "aqua", "cyanaqua" },
                { "cyan", "cyanaqua" },
                { "fuchsia", "magentaFunchsia" },
                { "magenta", "magentaFunchsia" },
                { "white", "white" },
                { "black", "black" },
                { "blue", "blue" },
                { "green", "green" },
                { "yellow", "yellow" },
                { "red", "red" },
                { "brown", "coconut" } // exemplo: se a API devolver 'Brown' e você quiser mapear pra coconut
            };
            if (alias.TryGetValue(normalized, out var mapped)) normalized = mapped;

            // busca na lista Chaves (case-insensitive)
            var found = Chaves.FirstOrDefault(c => string.Equals(c.Cor, normalized, StringComparison.OrdinalIgnoreCase));
            return found?.Componente;

        }

        //constrói frase com regras simples(não remover componentes especiais como "#" e "\" \"" ) 
        private string BuildPhraseFromComponents(System.Collections.Generic.List<string> componentes)
        {
            
        }

    }
}
