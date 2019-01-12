using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Pixelmon.Core.Exceptions;
using Pixelmon.Utils;
using fNbt;

namespace Pixelmon
{
    class Installer
    {

        public string ProfileName { get; set; } = "Pixelmon";
        public string ForgeVersion { get; set; } = "Forge 1.12.2";
        public string Adress { get; set; } = "pixelmon.craft.gg";
        public string Base64Icon { get; set; } = "iVBORw0KGgoAAAANSUhEUgAAAGQAAABkCAYAAABw4pVUAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAupSURBVHhe7V15bFxHGQ8p4a44BASv7X2765BAaYVEWu5wqKgcglIo+QtEhAgRkEZISImbgzpNQq6mSWzvvLWbtoG0CNWoREEFgThToISQIJGmOZrETdpcJoe989YmhcTL73v7rf3e29nDu/Psfc77ST+tvDvzffPN9+b6Zt54SogQIUKECBEiRIgQIUJc75jRLt8WN+UccH5cWKtjptyMz+64afWA22NCdsZMay2++37cTH8+ag4kprRlp3L2ELUilhwyqPJR0Y/HhTyLSs+OmUIO4nM3PpfGxeBtoYPGCGNL/5vw5C/Ak/5XVOJwQQXXSDj3PFrQxnhneiarDKFConsoitbQAQ6pKlI7ydlC/tEwM5/lIoQgGMnMO1BBD+HJfbmg0saJMWE9nUjJj3CRrlP0ZG+Ip6x7UCH93gqaMAr5U5o4cAmvH0Q7+uN4Kv+urJQJJlpqXzwpv8JFnfwwUvJLMLx+WkVxPjS7OzuNiz0Jkc2+AjOnNQrD65m7Z3bLt7IFkwg0XuCJ8xgbFPZSF8uWTALAGWgZP1MYGiDKk0aqP8YWBRjopmBQqtDAQLI3vs2azpYFE5hJrVMYFjjSzCsmMl+nB4xN0wqKTiSS1u38pz9ICPlllXHBoryGzxRVGJulFzTRgaPtqXbK+ip/qx80CMKQIExtizJmyn12MNIntCSt90DPblufkMMUseCfNIMG8Tpd9FXIfoog+BUVvinZ9wYKbqJV/DevE84/yD/rBxQszCsKFPGUomIe8+9JpbqRd0PHiwrd7ZxELzhQGMSu6pAhrE+yGdrRkhyYgUr/lUJvjiL9BU6qF+iqtikV1iuFHEyY8t6berKvYhO0wtiefQ1aRFvJLQUhrya6L7+Rs+gD7WeMRwgdi8w0Pnuon6enekbXQIvdMjvTM42u9IdotoI+ugvpjnnzuiisXbQjycXXDpTpM2XLQBTWM5xFL/AUdCgVaiKcfQo65jc9OPxaVlkWRsr6BJ7AJ8DhEVnCesG3LgJoWXVqdryjf5ez7KVI8T3Oqg80Ty/ZLGsgCmzFzPTisTjCixaR/jC6jr2QtbYWOaUwe0F2WtPq3icjrQeHoxvOKm1R0ZexC8YuUCmrnfJkLJV5L6upW0Q7Lt/TuOJopmHJgSyx8b7nFbaoKIdmtA+/msXoAwbzv6gVVk90Uf9ImNbbWUVdIt5l3QLbT6AbzEaWHbKdkWe8s19pl5vyNyxKHygK6uqjNZDGi7reSu3OTjNobDJH7W5ac8rlkOYNZwrs8hJ2trJEfUCh5quUVU06R9Vp3cLi6w4JMz3PLqOn3MaWCy6HNLYdd/2uYqJr8FYWqw/w8k9Uyqol5C1h0XUFOz4nrOdUZbYpZLah9eCIQyL3HrS/U6YlCuuSL+EZtJAzSoVVkMIKtJhi0aVBey1i8DaagSEfHSldCRlzZz184UZOoQfZ7FQjmd4GHRT5VZY7TxrMna3E2HJRmY6IGd+TrEEfcmdt1QqropDfZNElEUvKjyHtP1UyctNkueLjbdlXcvKq0SIG74K8AZUeFZt/6BlH1p9WprMp5HdZjT6gdcxRKquCmK3Ihu7s61h0USRE+huo8P+pZDiJru+3lchT4eZt56fDEXtVcksxuvGsyyFNq3qV6Wz6cZRV84Dew2KLIpGUH0VFj4StK+B2zloxIH8THH5VIassjc0XXQ5p/MFRZTrIf4nV6QX67dUqhVWR9iBKwR4z1N1UKVJ8iyWURCLZf3tcpC+oZFTKWOeAyyENrc8q08GOH7FavYDgLUqFVZDiTixWiXhq4P2qfOWIrudRFlEU8VRmJ1q7Mv+YSDMtp0NAjHeKdNbXWLVe6Ay3054Bi1WCZlOqfBWwl0UUBex4XpGvKkaWPOt2SEfhir25czDCqvVC5xqkKTXYyGKVQL+7WZWvHFHGl1lEUaAV/UKVtxrS+sPlkK2X3GmwlmG1+oEmusOlrAYmUul3slgleJ2hzFuKcEgfiygKOOQpVd5q6HUIreCdv6M8naxWP9DUTaeyWhhLpj/AYpVAmrnePJVR/oFFFAXs0HYow9tlFTgkKb/IavUDCtY7lVVDPJ3/jpmZeeUOodEKnNYqKhmlaAj5bRZRFEin5xyAalB3dllCXvXtjBcBFfQ9V4HGRHkN3VBX1Bx4M4srC1qBq2UVobBOlNtvMLrlu5R5qyAN4AUOab/sTLOH1fqDRCrzOYeyMVDup2ksi6kc9pkv+Xu1TA+FHGwxB2dzzqKo7aFy09jsjvgS48n0yO/oDdayWn9Ay39ngSpgPypgEVUsixgzKPgIh/5YIdvJ3mhn5n2cpSTQ6vYp8ldF2gNxOiPiWRj6fn6XTymW79d9OIRGK3DofgTyeyH7ClrOeRrAwe9UeqyH3sAtKGsNbFr9gtshyw45fpdDFUeyawEq+9fOQino6yG0qtGWnQpHHlCUt2pGVhx1OaSx7djIb9D1O9bsL+CQ5c5CjdDnQ2i1AmXUeuQV03Y7duV0SPPqk6NphFzKqv0FbUM6C5ZT7u8htFqB7mMOreALyl0Do5v6XM4guo4D+XiSvgA0vcwp9fcQmg7QhTS09hmpKE1sur+3wCHG1pEdw/5aJjJjBhTeT1M6vw6h6QLvp/TlK1EbRbogZEL76+im8ml2chGCD7ryItqVaeA/qwMGcFTKQt3dVJ7enUKic0AHF3JJggtyAirwcTIIrc8+Tkovu/DPFSN36Fn+y1E52kk7g16HRNeN7qcbQr6bixNA5NY2i2BIQXyJ1zypmJn5dKnTJRQOQVqswOV+rwzdNB4sHMyJRjvHsIQ8zcUKHnK7ghVWopBXMYk4gtbzZ/y9E63pl/jcg+8uu9L5SaFuHZFlhx1p5A42LzigACOM64Izyp59qicWax3Na18cSWO/Th0Y0GvBZmaeH9NQ34kZVOPyI4UOaaVt29EI74z2wSa2tr6RMK2b0c8/7TIyQPQers7T9SqCsA6zufWLkdeCKzjsVq+0D1Z7wiR5Rh84P5IO41qSza5PYIxQvxYcIKKSC94FydOO7o4uBrN0mwWbXl+gFzN5FuQyLnCkcWPlcaUziO7Ylbw6lt3QcUMsKb+FljE+N436TIreqhxBpNZBrSefFj3BXq6C+oJ9GTIFHR2GBZHNa19SOiJP59jBXM9VUH/IvfYWXKc0rzutdEKeRV7y/BSbX5+wL0qu4rD0hBJdkHdbtoCYbXlOllB3daXeI982pj9w7vUw8gln4euVdEia3hNUOsHBqPKlnPKH8+oH9uo8vRiO+U+hIfXB2NaL2ciKw0oHOGm/2ClUMuQytjY4oJA0jPlboTETSHRR9Fqa9zioipGlWHM4zlw5aZjpD7KZAUOPfX0sbRyd9xo13jS2XDoeWV6+VdjEuEGH4lRyYsIaGNftWj9AYwudUEGLGb/QOZEuPBDWLgr7z1p8+MaGZc8NKR3gZCvGjY3n1PKIkMdmBR92nCv33uKeAkN1UliX8CSb3gsKmlcdu1PpBAdpCqyUyYTcRSxucoEqC13Zfai8Z/AkV/USppvyDOTswNh1V6kD2I0rj/1c5QgiLQ7VskdJl1uyqMmLpm0Db8HM7E5U6FLM8R8D96GCz+Fv9xUXuS7ocsy0joFPwaGbqMUh7ywWVR5ze25oWH7kkssZGDPU01sPhTzLUq5jYAClIB6NQ/xNzWhec/zWSOuBYXIGHZg2NvWpHeAhPSwsIoRuRNpObKVZl3cVXop0qQFnD+EHYsn0w6qKL8YWMdTMWUP4AeoGaTxSVb6XSHeUs4XwE/QyKsaGSrabU5wlhN+AQ9oUDvBQ3s3JQ/gNuuoJlV5isSqv0dSck4cYD9jXhZtWptAZtkP2c7IQ4wl0XcWuw93ASUKMNzCbKrgXJSYyd/DPIcYb9H+onNsFaDVXqr25LoQm0Gt7jhbyJ/46xEQinvvH+tRCVvBXISYS+VU8XfTPX4WYaNBdKjquow0RIkSIECFChAgRIkSI+sSUKf8H6OHjTIJtwzYAAAAASUVORK5CYII=";

        /// <summary>
        /// Current installation path.
        /// </summary>
        public string MinecraftInstallationPath { get; set; }

        /// <summary>
        /// Pixelmon content path.
        /// </summary>
        public string PixelmonPath { get; set; }

        /// <summary>
        /// Current Java path.
        /// </summary>
        public FileInfo JavaPath { get; set; }

        /// <summary>
        /// Minecraft's application data path.
        /// </summary>
        public string AppdataPath { get; set; }

        /// <summary>
        /// Defines whether or not Pixelmon is installed.
        /// </summary>
        public bool Installed { get; private set; } = false;

        /// <summary>
        /// Sets up the installation paths.
        /// </summary>
        public void Setup()
        {
            Logger.Log(LogLevel.Debug, "Définition des chemins...");
            this.MinecraftInstallationPath = this.MinecraftInstallationPath ?? this.GetDefaultMinecraftInstallationPath();
            this.AppdataPath = this.AppdataPath ?? this.GetAppdataPath();
            this.PixelmonPath = this.PixelmonPath ?? this.GetPixelmonPath();

            Logger.Log(LogLevel.Debug, "Vérification de l'installation...");
            if (!this.CheckMinecraftInstallation())
                throw new Exception("Il y a un problème avec votre installation de Minecraft.");

            this.JavaPath = this.JavaPath ?? this.FindJavaExecutable();
        }

        /// <summary>
        /// Starts Minecraft.
        /// </summary>
        /// <return>Minecraft's process PID.</return>
        public int StartMinecraft()
        {
            if (!this.CheckMinecraftInstallation(false))
                throw new MinecraftNotFoundException(this.MinecraftInstallationPath);

            return Process.Start(Path.Combine(this.MinecraftInstallationPath, "MinecraftLauncher.exe")).Id;
        }

        #region Installation

        /// <summary>
        /// Installs Pixelmon and configures it.
        /// </summary>
        public async Task InstallAsync()
        {
            try
            {
                await this.InstallForgeAsync();

                Logger.Log("Configuration de forge...");
                this.CustomizeForge();
                this.RenameForgeVersion();
                Logger.Log("Forge a été configuré.");

                this.InstallMods();
                this.AddServer();
                this.Installed = true;
            }
            catch (Exception ex)
            {
                this.Installed = false;
                throw ex;
            }
        }

        /// <summary>
        /// Installs the mods.
        /// </summary>
        private void InstallMods()
        {
            Logger.Log("Installation des mods...");
            try
            {
                string source = Path.Combine(this.PixelmonPath, "mods");
                string dest = Path.Combine(this.AppdataPath, "mods");

                Logger.Log(LogLevel.Debug, $"Source : ${source}");
                Logger.Log(LogLevel.Debug, $"Destination : ${dest}");
                this.DirectoryCopy(source, dest, true);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Danger, "Les mods n'ont pas pu être installés.");
                Logger.Log(LogLevel.Debug, ex.ToString());
                throw ex;
            }
            Logger.Log("Les mods ont été installés.");
        }

        /// <summary>
        /// Adds server to the list.
        /// </summary>
        public void AddServer()
        {
            try
            {
                Logger.Log("Ajout du serveur Pixelmon dans la liste des serveurs...");
                string serversPath = Path.Combine(this.AppdataPath, "servers.dat");

                NbtFile servers = new NbtFile();

                if (!File.Exists(serversPath))
                {
                    Logger.Log(LogLevel.Warning, "Le fichiers contenant la liste des serveurs n'existe pas.");
                    servers.RootTag = new NbtCompound("");
                    servers.RootTag.Add(new NbtList("servers"));
                }
                else
                {
                    Logger.Log(LogLevel.Debug, "Lecture du fichier des serveurs...");
                    servers.LoadFromFile(serversPath);
                }


                Logger.Log(LogLevel.Debug, "Ajout de Pixelmon...");
                NbtList serverInfo = (NbtList)servers.RootTag["servers"];
                NbtCompound pixelmonInfo = new NbtCompound
                {
                    new NbtString("ip", this.Adress),
                    new NbtString("name", this.ProfileName),
                    new NbtString("icon", this.Base64Icon),
                    new NbtByte("acceptTextures", (byte)1)
                };
                serverInfo.Add(pixelmonInfo);
                servers.RootTag = (NbtCompound)serverInfo.Parent;

                servers.SaveToFile(serversPath, NbtCompression.None);
                Logger.Log("Pixelmon a été ajouté à la liste.");
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Warning, "Une erreur inconnue est survenue lors de l'ajout de Pixelmon dans la liste.");
                Logger.Log(LogLevel.Warning, $"Il faudra ajouter manuellement l'adresse '{this.Adress}'.");
                throw ex;
            }
        }

        /// <summary>
        /// Installs forge thanks to the installer.
        /// </summary>
        private async Task<bool> InstallForgeAsync()
        {
            Logger.Log("Installation de Forge...");

            int hwnd = 0;
            Logger.Log(LogLevel.Debug, "Création du processus...");
            ProcessStartInfo startInfo = new ProcessStartInfo(this.JavaPath.FullName)
            {
                Arguments = this.GetProcessArguments(),
                WorkingDirectory = this.PixelmonPath,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };
            Logger.Log(LogLevel.Debug, $"Java : {this.JavaPath.FullName}");
            Logger.Log(LogLevel.Debug, $"Arguments : {startInfo.Arguments}");
            Process forge = Process.Start(startInfo);

            // Gets the Forge installer window
            Logger.Log(LogLevel.Debug, "Attente de la fenêtre Forge...");
            hwnd = await this.WaitForWindowAsync("Mod system installer");
            WindowsApi.SetForegroundWindow((IntPtr)hwnd);

            // Sets up installation
            Logger.Log(LogLevel.Debug, "Envoi des paramètres et des touches à la fenêtre...");
            SendKeys.SendWait("+{TAB}{ENTER}");
            await Task.Delay(100);
            SendKeys.SendWait("^A{DEL}");
            SendKeys.SendWait(this.AppdataPath);
            SendKeys.SendWait("{ENTER}");
            await Task.Delay(100);
            SendKeys.SendWait("{TAB}{ENTER}");

            // Gets the Forge installer confirmation message
            Logger.Log(LogLevel.Debug, "En attente de la fenêtre de confirmation...");
            hwnd = await this.WaitForWindowAsync("Complete");
            WindowsApi.SetForegroundWindow((IntPtr)hwnd);

            // Closes it
            SendKeys.SendWait("{ENTER}");

            Logger.Log(LogLevel.Debug, "En attente de la fermeture de l'installateur de Forge...");
            await forge.WaitForExitAsync();
            if (!forge.HasExited || !this.FindForgeProfile().Exists || !this.FindLauncherProfiles().Exists)
            {
                forge.Kill();
                Logger.Log(LogLevel.Warning, "Une erreur est survenue lors de l'installation de Forge.");
                throw new Exception("There was an error during the installation");
            }

            Logger.Log("Installation de Forge terminée.");
            return true;
        }

        /// <summary>
        /// Renames the forge version so it has a readable name.
        /// </summary>
        private void RenameForgeVersion()
        {
            Logger.Log(LogLevel.Debug, "Renommage de Forge...");
            FileInfo profile = this.FindForgeProfile();

            Logger.Log(LogLevel.Debug, $"Renommage du profil {profile.Name} en {this.ForgeVersion + ".json"}...");
            string newProfile = Path.Combine(profile.DirectoryName, this.ForgeVersion + ".json");
            if (!File.Exists(newProfile))
                File.Move(profile.FullName, newProfile);

            Logger.Log(LogLevel.Debug, $"Renommage du dossier du profil...");
            string newDirectory = profile.DirectoryName.Replace(profile.Name.Replace(".json", null), this.ForgeVersion);
            if (!Directory.Exists(newDirectory))
                Directory.Move(profile.DirectoryName, newDirectory);

            Logger.Log(LogLevel.Debug, $"Modification du contenu du fichier de profil...");
            string path = Path.Combine(newDirectory, this.ForgeVersion + ".json");
            JObject json = JObject.Parse(File.ReadAllText(path));
            json["id"] = this.ForgeVersion;

            Logger.Log(LogLevel.Debug, $"Écriture...");
            File.WriteAllText(path, json.ToString());
            Logger.Log(LogLevel.Debug, "Forge a été renommé.");
        }

        /// <summary>
        /// Customizes this forge version by renaming it and setting it as the last one used.
        /// </summary>
        private bool CustomizeForge()
        {
            Logger.Log(LogLevel.Debug, "Modification de Forge...");
            string profiles = this.FindLauncherProfiles().FullName;

            JObject json = JObject.Parse(File.ReadAllText(profiles));
            JObject profile = (JObject)json["profiles"]["forge"];

            this.SetKeyOrReplace(ref profile, "name", this.ProfileName);
            this.SetKeyOrReplace(ref profile, "lastVersionId", this.ForgeVersion);
            this.SetKeyOrReplace(ref profile, "type", "custom");
            this.SetKeyOrReplace(ref profile, "icon", "Furnace_On");
            this.SetKeyOrReplace(ref profile, "lastUsed", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK"));

            Logger.Log(LogLevel.Debug, $"Écriture...");
            File.WriteAllText(profiles, json.ToString());
            Logger.Log(LogLevel.Debug, "Forge a été modifié.");
            return true;
        }

        /// <summary>
        /// Replaces or adds a key.
        /// </summary>
        private void SetKeyOrReplace(ref JObject profile, string key, string val)
        {
            Logger.Log(LogLevel.Debug, $"Ajout de {val} à {key}...");
            if (profile[key] != null)
                profile[key] = val;
            else
                profile.Property("name").AddAfterSelf(new JProperty(key, val));
        }

        /// <summary>
        /// Gets the installation command line.
        /// </summary>
        private string GetProcessArguments()
        {
            return $"-jar \"{Path.Combine(this.PixelmonPath, "forge.jar")}\"";
        }

        /// <summary>
        /// Waits for a window to open and return its handle.
        /// </summary>
        /// <param name="name">Window name.</param>
        private async Task<int> WaitForWindowAsync(string name)
        {
            int hwnd = 0;
            int tries = 0;
            int maxTries = 50000;
            do
            {
                hwnd = WindowsApi.FindWindow(null, name);
                await Task.Delay(50);
            }
            while (hwnd == 0 || tries >= maxTries);

            if (tries >= maxTries)
                throw new ForgeTimeoutException();

            return hwnd;
        }

        #endregion

        #region Paths

        /// <summary>
        /// Checks if Minecraft is properly installed.
        /// </summary>
        /// <param name="path">Minecraft installation path.</param>
        private bool CheckMinecraftInstallation(bool checkAppdata = true)
        {
            Logger.Log(LogLevel.Debug, "Vérification du répertoire d'installation de Minecraft...");
            Logger.Log(LogLevel.Debug, $"Répertoire : {this.MinecraftInstallationPath}");
            if (!Directory.Exists(this.MinecraftInstallationPath))
                throw new MinecraftNotFoundException(this.MinecraftInstallationPath);

            if (!File.Exists(Path.Combine(this.MinecraftInstallationPath, "MinecraftLauncher.exe")))
                throw new MinecraftNotFoundException(this.MinecraftInstallationPath);

            if (checkAppdata)
            {
                Logger.Log(LogLevel.Debug, "Vérification du répertoire des données de Minecraft...");
                Logger.Log(LogLevel.Debug, $"Répertoire : {this.AppdataPath}");
                if (!Directory.Exists(this.AppdataPath))
                    throw new AppdataNotFoundException(this.AppdataPath);
            }

            Logger.Log(LogLevel.Debug, "Vérification des fichiers Pixelmon...");
            Logger.Log(LogLevel.Debug, $"Répertoire : {this.PixelmonPath}");
            if (!Directory.Exists(this.PixelmonPath))
                throw new PixelmonNotFoundException(this.PixelmonPath);

            return true;
        }

        /// <summary>
        /// Gets the Pixelmon path.
        /// </summary>
        private string GetPixelmonPath()
        {
            return Path.Combine(
                Environment.CurrentDirectory,
                "pixelmon");
        }

        /// <summary>
        /// Gets the default application data path (%appdata%/.minecraft)
        /// </summary>
        private string GetAppdataPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ".minecraft");
        }

        /// <summary>
        /// Return the default Minecraft installation path.
        /// </summary>
        public string GetDefaultMinecraftInstallationPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), 
                "Minecraft");
        }

        /// <summary>
        /// Gets the installation path for this instance.
        /// </summary>
        private string GetInstallationPath()
        {
            return this.MinecraftInstallationPath ?? this.GetDefaultMinecraftInstallationPath();
        }

        /// <summary>
        /// Gets the Java executable path.
        /// </summary>
        private FileInfo FindJavaExecutable()
        {
            string runtime = Path.Combine(this.GetInstallationPath(), "runtime");
            string result = Directory.GetFiles(runtime, "*", SearchOption.AllDirectories)
                            .Where(file => file.Contains("java.exe"))
                            .First();

            if (result == null)
                throw new FileNotFoundException("The Java executable could not be found.");

            return new FileInfo(result);
        }

        /// <summary>
        /// Gets the Forge profile path.
        /// </summary>
        private FileInfo FindForgeProfile()
        {
            string version = Path.Combine(this.AppdataPath, "versions");
            string result = Directory.GetFiles(version, "*", SearchOption.AllDirectories)
                            .Where(file => file.ToLower().Contains("forge"))
                            .First();

            if (result == null)
                throw new FileNotFoundException("The profile could not be found.");

            return new FileInfo(result);
        }

        /// <summary>
        /// Gets the launcher profiles.
        /// </summary>
        private FileInfo FindLauncherProfiles()
        {
            string result = Directory.GetFiles(this.AppdataPath, "*", SearchOption.AllDirectories)
                            .Where(file => file.Contains("launcher_profiles.json"))
                            .First();

            if (result == null)
                throw new FileNotFoundException("The launcher profiles could not be found.");

            return new FileInfo(result);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Copies one directory into another.
        /// </summary>
        /// <see cref="https://docs.microsoft.com/fr-fr/dotnet/standard/io/how-to-copy-directories"/>
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                Logger.Log(LogLevel.Debug, $"Le répertoire source n'existait pas.");
                throw new PixelmonNotFoundException(sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Logger.Log(LogLevel.Debug, $"Création du répertoire {destDirName}...");
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);

                Logger.Log(LogLevel.Debug, $"Copie de {file.Name}...");
                if (File.Exists(temppath))
                {
                    Logger.Log(LogLevel.Warning, $"Le mod ${file.Name} était déjà installé.");
                    continue;
                }
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        #endregion
    }
}
