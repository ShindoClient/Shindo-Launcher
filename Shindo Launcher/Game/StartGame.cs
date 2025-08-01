using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using Shindo_Launcher.Utils;
using Shindo_Launcher.Views;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shindo_Launcher.Game
{
    internal class StartGame
    {

        private static readonly string shindoPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".shindo");

        public static async Task start()
        {
            try
            {
                var myPath = new MinecraftPath(shindoPath)
                {
                    BasePath = shindoPath,
                    Library = Path.Combine(shindoPath, "libraries"),
                    Versions = Path.Combine(shindoPath, "versions")
                };

                var ini = new INIFile(Path.Combine(shindoPath, "launcher.ini"));
                int ramMb = int.Parse(ini.Read("Launcher", "RAM", "4096"));

                var launcher = new MinecraftLauncher(myPath);

                var launchOption = new MLaunchOption
                {
                    Session = MSession.CreateOfflineSession("Player"),
                    JavaPath = Path.Combine(shindoPath, "java", "bin", "java.exe"),
                    MaximumRamMb = ramMb,
                    ScreenWidth = 1600,
                    ScreenHeight = 900,
                    GameLauncherName = "ShindoLauncher",
                    GameLauncherVersion = "1.2"
                };

                MainWindow.statusLabel!.Text = "Iniciando Minecraft...";
                Console.WriteLine("Iniciando Minecraft...");
                var process = await launcher.InstallAndBuildProcessAsync("ShindoClient", launchOption);

                // ✅ Ajustes para rodar em background sem CMD
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                // ✅ Handlers para capturar logs
                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine("[MC] " + e.Data);
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine("[MC-ERR] " + e.Data);
                };

                // Inicia o processo
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                MainWindow.statusLabel!.Text = "Minecraft iniciado com sucesso";
                Console.WriteLine("Minecraft iniciado com sucesso (rodando em background).");

                await process.WaitForExitAsync();

                MainWindow.statusLabel!.Text = "Minecraft finalizado";
                MainWindow.playButton!.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MainWindow.statusLabel!.Text = "Erro ao iniciar Minecraft";
                MainWindow.playButton!.IsEnabled = true;
                Console.WriteLine("Erro ao iniciar Minecraft:");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
