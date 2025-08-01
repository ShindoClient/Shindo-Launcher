using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Shindo_Launcher.Utils;
using System;
using System.IO;
using System.Management;
using System.Threading.Tasks;

namespace Shindo_Launcher.Views;

public partial class MainWindow : Window
{
    public static TextBlock? statusLabel;
    public static TextBlock? attLabel;
    public static Button? playButton;

    private StackPanel? loadingPanel;
    private StackPanel? mainPanel;
    private StackPanel? optionsPanel;

    private Slider? ramSlider;
    private TextBlock? ramValueLabel;

    private readonly string iniPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".shindo", "launcher.ini");
    private INIFile ini;

    public MainWindow()
    {
        InitializeComponent();

        if (!File.Exists(iniPath))
        {
            File.WriteAllText(iniPath, "[Launcher]\nRAM=4096");
        }
        ini = new INIFile(iniPath);

        // Pega os elementos visuais do XAML
        statusLabel = this.FindControl<TextBlock>("StatusLabel");
        attLabel = this.FindControl<TextBlock>("AttLabel");
        playButton = this.FindControl<Button>("PlayButton");

        loadingPanel = this.FindControl<StackPanel>("LoadingPanel");
        mainPanel = this.FindControl<StackPanel>("MainPanel");
        optionsPanel = this.FindControl<StackPanel>("OptionsPanel");

        ramSlider = this.FindControl<Slider>("RamSlider");
        ramValueLabel = this.FindControl<TextBlock>("RamValueLabel");

        // Configura slider baseado na RAM do sistema
        SetUpRamSlider();

        // Carrega RAM salva no ini (ou padrão)
        LoadRamFromIni();

        _ = RunUpdaterAndShowUIAsync();
    }

    public static ulong GetTotalMemory()
    {
        ulong totalMemory = 0;
        using (var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
        {
            foreach (var obj in searcher.Get())
            {
                totalMemory = (ulong)obj["TotalPhysicalMemory"];
            }
        }
        return totalMemory;
    }

    private void SetUpRamSlider()
    {
        ulong totalMemory = GetTotalMemory();
        int totalMB = (int)(totalMemory / (1024 * 1024));

        // Deixa o slider com mínimo 1024MB e máximo 80% da RAM
        ramSlider!.Minimum = 1024;
        ramSlider.Maximum = (int)(totalMB * 0.8);
        ramSlider.TickFrequency = 512;
    }

    private void LoadRamFromIni()
    {
        string savedRam = ini.Read("Launcher", "RAM", "");
        if (!string.IsNullOrEmpty(savedRam) && int.TryParse(savedRam, out int ram))
        {
            ramSlider!.Value = ram;
            ramValueLabel!.Text = $"RAM Selecionada: {ram} MB";
        }
        else
        {
            // Se não tiver valor salvo, usa 4096MB como padrão
            ramSlider!.Value = 4096;
            ramValueLabel!.Text = "RAM Selecionada: 4096 MB";
        }
    }

    private async Task RunUpdaterAndShowUIAsync()
    {
        await RunUpdaterAsync();

        await Task.Delay(500);

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            // Fade out loading panel
            loadingPanel!.Opacity = 1;
            mainPanel!.Opacity = 0;
            loadingPanel.IsVisible = true;
            mainPanel.IsVisible = true;
        });

        await AnimateFade(loadingPanel!, mainPanel!);

        statusLabel!.Text = "Pronto para iniciar!";
        playButton!.IsEnabled = true;
    }

    private async Task RunUpdaterAsync()
    {
        await Updater.Updater.update();
    }

    private async Task AnimateFade(Control fadeOut, Control fadeIn)
    {
        for (double i = 1; i >= 0; i -= 0.05)
        {
            fadeOut.Opacity = i;
            await Task.Delay(15);
        }

        fadeOut.IsVisible = false;
        fadeIn.IsVisible = true;

        for (double i = 0; i <= 1; i += 0.05)
        {
            fadeIn.Opacity = i;
            await Task.Delay(15);
        }
    }

    private async Task Play(object? sender, RoutedEventArgs e)
    {
        playButton!.IsEnabled = false;
        await Game.StartGame.start();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (playButton != null && playButton.IsEnabled)
        {
            playButton.IsEnabled = false;
            _ = Play(sender, e);
        }
    }

    private void Button_Click_2(object? sender, RoutedEventArgs e)
    {
        mainPanel!.IsVisible = false;
        optionsPanel!.IsVisible = true;
    }

    // Fecha o painel de opções e salva a RAM
    private void Button_Click_3(object? sender, RoutedEventArgs e)
    {
        optionsPanel!.IsVisible = false;
        mainPanel!.IsVisible = true;

        int selectedRam = (int)ramSlider!.Value;
        ini.Write("Launcher", "RAM", selectedRam.ToString());
        Console.WriteLine($"RAM escolhida: {selectedRam} MB");
    }

    // Atualiza o label quando o slider é movido
    private void Slider_ValueChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        ramValueLabel!.Text = $"RAM Selecionada: {ramSlider!.Value} MB";
    }
}