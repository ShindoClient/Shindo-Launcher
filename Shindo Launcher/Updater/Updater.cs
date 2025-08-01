using Shindo_Launcher.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shindo_Launcher.Updater
{
    internal class Updater
    {
        static readonly string baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".shindo");
        static readonly string versionsDir = Path.Combine(baseDir, "versions");
        static readonly string versionFile = Path.Combine(baseDir, "version.json");
        static readonly string tempDir = Path.Combine(Path.GetTempPath(), "shindo_updater");
        static readonly HttpClient httpClient = new();

        public static async Task update()
        {
            Directory.CreateDirectory(baseDir);
            Directory.CreateDirectory(tempDir);

            Log("Verificando atualizações...", ConsoleColor.Cyan);

            try
            {
                string remoteJson = await httpClient.GetStringAsync("https://github.com/ShindoClient/RESOURCES/releases/download/latest/version.json");
                var remoteVersion = JsonSerializer.Deserialize(remoteJson, VersionInfoJsonContext.Default.VersionInfo);

                VersionInfo? localVersion = null;
                if (File.Exists(versionFile))
                {
                    string localJson = await File.ReadAllTextAsync(versionFile);
                    localVersion = JsonSerializer.Deserialize(localJson, VersionInfoJsonContext.Default.VersionInfo);
                }

                if (localVersion == null || remoteVersion.version != localVersion.version)
                {
                    MainWindow.attLabel!.Text = "Nova atualização encontrada!";
                    Log($"Nova versão encontrada: {remoteVersion.version}", ConsoleColor.Yellow);

                    MainWindow.attLabel!.Text = "Baixando ...";
                    await DownloadAndExtract(remoteVersion.client_url, Path.Combine(versionsDir, "ShindoClient"));
                    await DownloadAndExtract(remoteVersion.java_url, Path.Combine(baseDir, "java"));

                    await File.WriteAllTextAsync(versionFile, remoteJson);

                    MainWindow.attLabel!.Text = "Atualização concluida!";
                    Log("Atualização concluída com sucesso!", ConsoleColor.Green);
                }
                else
                {
                    MainWindow.attLabel!.Text = "O client ja esta atualizado!";
                    Log("Client já está atualizado.", ConsoleColor.Green);
                }
            }
            catch (Exception ex)
            {
                MainWindow.attLabel!.Text = "Erro durante a atualização!";
                Log($"Erro durante a atualização: {ex.Message}", ConsoleColor.Red);
            }

            Cleanup();
        }

        static async Task DownloadAndExtract(string url, string extractPath)
        {
            string fileName = Path.GetFileName(url);
            string tempZip = Path.Combine(tempDir, fileName);

            Log($"Baixando {fileName}...", ConsoleColor.Cyan);
            Directory.CreateDirectory(extractPath);

            using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                var canReportProgress = totalBytes != -1;

                using var stream = await response.Content.ReadAsStreamAsync();
                using var fs = new FileStream(tempZip, FileMode.Create, FileAccess.Write, FileShare.None);

                var buffer = new byte[8192];
                long totalRead = 0;
                int read;
                var sw = Stopwatch.StartNew();

                while ((read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
                {
                    await fs.WriteAsync(buffer.AsMemory(0, read));
                    totalRead += read;

                    if (canReportProgress)
                    {
                        double percent = (double)totalRead / totalBytes * 100;
                        Console.Write($"\r{fileName}: {percent:F1}% ({totalRead / 1024 / 1024} MB de {totalBytes / 1024 / 1024} MB)");
                    }
                }

                Console.WriteLine();
                Log($"Download de {fileName} concluído em {sw.Elapsed.TotalSeconds:F1}s", ConsoleColor.Gray);
            }

            Log($"Extraindo {fileName} para {extractPath}...", ConsoleColor.DarkYellow);
            ZipFile.ExtractToDirectory(tempZip, extractPath, true);
        }

        static void Log(string message, ConsoleColor color)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.ForegroundColor = color;
            Console.WriteLine($"[{timestamp}] {message}");
            Console.ResetColor();
        }

        static void Cleanup()
        {
            try
            {
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                    Log("Diretório temporário limpo.", ConsoleColor.DarkGray);
                }
            }
            catch
            {
                Log("Falha ao limpar diretório temporário.", ConsoleColor.DarkRed);
            }
        }
    }
    public record VersionInfo(string version, string client_url, string java_url);
}
