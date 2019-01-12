using Pixelmon.Utils;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Pixelmon.Core.Exceptions;
using Console = Pixelmon.Utils.ConsoleHelper;

namespace Pixelmon
{
    class Program
    {

        public static bool Debug { get; set; }
        public static string MinecraftPath { get; set; }
        public static string PixelmonPath { get; set; }

        [STAThread]
        static void Main(string[] args)
        {
            Program.ParseArguments(args);
            Program.WriteHeader();

            Logger.Log("Bienvenue sur l'installateur automatique du serveur Pixelmon.");
            Logger.Log("Cet assistant va installer Forge, les mods nécessaires, et va ajouter Pixelmon à la liste des serveurs.");
            Console.NewLine();

            Console.Pause("Veuillez appuyer sur une touche pour démarrer l'installation.");

            Program.InstallWizardAsync().GetAwaiter().GetResult();
            Logger.Log(LogLevel.Debug, "Fin du programme d'installation.");

            Console.Pause();
        }

        /// <summary>
        /// Writes header.
        /// </summary>
        private static void WriteHeader()
        {
            Console.SetTile("Assistant d'installation de Pixelmon");
            Console.WriteCenteredLine(@"

__________.__              .__                         
\______   \__|__  ___ ____ |  |   _____   ____   ____  
 |     ___/  \  \/  // __ \|  |  /     \ /  _ \ /    \ 
 |    |   |  |>    <\  ___/|  |_|  Y Y  (  <_> )   |  \
 |____|   |__/__/\_ \\___  >____/__|_|  /\____/|___|  /
                   \/    \/           \/            \/ 
[ Installateur par Hawezo - https://github.com/hawezo ]

");
        }

        /// <summary>
        /// Parses arguments.
        /// </summary>
        private static void ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string argument = args[i];
                switch (argument.ToLower())
                {
                    case "--d":
                    case "--debug":
                        Program.Debug = true;
                        break;

                    case "--m":
                    case "--minecraft":
                        if (args.Length >= i + 1 && args[i + 1] != null)
                        {
                            Program.MinecraftPath = args[i + 1].Replace("\"", null);
                            Logger.Log(LogLevel.Debug, $"Le chemin passé en argument pour --minecraft est '{Program.MinecraftPath}'.");
                        }
                        else
                            Logger.Log(LogLevel.Debug, $"Le chemin passé en argument pour --minecraft est invalide.");
                        break;

                    case "--p":
                    case "--pixelmon":
                        if (args.Length >= i + 1 && args[i + 1] != null)
                        {
                            Program.PixelmonPath = args[i + 1].Replace("\"", null);
                            Logger.Log(LogLevel.Debug, $"Le chemin passé en argument pour --pixelmon est '{Program.PixelmonPath}'.");
                        }
                        else
                            Logger.Log(LogLevel.Debug, $"Le chemin passé en argument pour --pixelmon est invalide.");
                        break;
                }
            }
        }

        /// <summary>
        /// Installs the Pixelmon files.
        /// </summary>
        [STAThread]
        async static Task InstallWizardAsync()
        {
            Installer installer = new Installer();

            Logger.Log(LogLevel.Debug, "Vérification des arguments de ligne de commande...");
            if (Program.MinecraftPath != null)
                installer.MinecraftInstallationPath = Program.MinecraftPath;

            if (Program.PixelmonPath != null)
                installer.PixelmonPath = Program.PixelmonPath;

            do
            {
                try
                {
                    Logger.Log(LogLevel.Debug, "Configuration...");
                    installer.Setup();

                    installer.AddServer();

                    Logger.Log(LogLevel.Debug, "Installation...");
                    await installer.InstallAsync();
                    
                    if (Program.AskToStartMinecraft())
                        installer.StartMinecraft();
                }

                catch (MinecraftNotFoundException ex)
                {
                    Logger.Log(LogLevel.Warning, $"Minecraft n'a pas été trouvé dans le chemin suivant '{ex.GivenPath}'.");

                    do
                    {
                        Logger.Log(LogLevel.Info, "Veuillez indiquer le répertoire d'installation de Minecraft.");
                        Logger.Log(LogLevel.Info, $"Par défaut, '{installer.GetDefaultMinecraftInstallationPath()}'.");
                        installer.MinecraftInstallationPath = Program.AskFolder();
                    }
                    while (installer.MinecraftInstallationPath == null);

                }

                catch (AppdataNotFoundException ex)
                {
                    Logger.Log(LogLevel.Debug, $".minecraft n'a pas été trouvé dans le chemin suivant '{ex.GivenPath}'.");
                    Logger.Log(LogLevel.Warning, "Les fichiers de Minecraft n'ont pas été trouvés. Cela veut dire que vous ne l'avez pas encore lancé.");
                    if (Program.Ask("Voulez-vous le lancer maintenant ? Vous pourrez relancer cet installateur une fois le jeu lancé."))
                        installer.StartMinecraft();
                    Program.Exit();
                }

                catch (PixelmonNotFoundException ex)
                {
                    Logger.Log(LogLevel.Warning, $"Pixelmon n'a pas été trouvé dans le chemin suivant '{ex.GivenPath}'.");

                    do
                    {
                        Logger.Log(LogLevel.Info, "Veuillez indiquer le répertoire contenant les fichiers de Pixelmon.");
                        installer.PixelmonPath = Program.AskFolder();
                    }
                    while (installer.PixelmonPath == null);
                }

                catch (ForgeTimeoutException)
                {
                    Logger.Log(LogLevel.Warning, "Le délais d'attente de Forge est dépassé.");
                    if (!Program.AskRetry())
                        Program.Exit();
                }

                catch (Exception ex)
                {
                    Logger.Log(LogLevel.Danger, "Une erreur d'origine inconnue est survenue pendant l'installation.");
                    Logger.Log(LogLevel.Debug, ex.ToString());
                    if (!Program.AskRetry())
                        Program.Exit();
                }


            } while (!installer.Installed);
        }

        /// <summary>
        /// Asks for a folder to the user.
        /// </summary>
        [STAThread]
        static string AskFolder(string root = null)
        {

            Logger.Log(LogLevel.Debug, "Lancement du dialogue de choix de répertoire...");
            FolderBrowserDialog dialog = new FolderBrowserDialog()
            {
                SelectedPath = root
            };

            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                if (Program.Ask("Voulez-vous annuler l'installation ?"))
                {
                    Logger.Log(LogLevel.Debug, "L'utilisateur a annulé l'installation.");
                    Program.Exit();
                }
                else return null;
            }

            Logger.Log(LogLevel.Debug, $"Répertoire choisi : '${dialog.SelectedPath}'.");
            return dialog.SelectedPath;
        }

        /// <summary>
        /// Asks for a retry when the installation failed.
        /// </summary>
        static bool AskRetry()
        {
            return Program.Ask("Voulez-vous réessayer l'installation ?");
        }

        /// <summary>
        /// Asks to the user if he wants to start Minecraft.
        /// </summary>
        static bool AskToStartMinecraft()
        {
            return Program.Ask("Voulez-vous lancer Minecraft ?");
        }

        /// <summary>
        /// Asks for a retry when the installation failed.
        /// </summary>
        static bool Ask(string message)
        {
            Logger.Log(LogLevel.Ask, message);
            ConsoleKeyInfo result;
            do {
                result = Console.ReadKey(true);
            }
            while (
                result.KeyChar != 'N' && 
                result.KeyChar != 'n' && 
                result.KeyChar != 'Y' && 
                result.KeyChar == 'y' &&
                result.KeyChar != 'O' &&
                result.KeyChar != 'o');

            bool choice = result.KeyChar == 'N' || result.KeyChar == 'n' ? false : true;
            Logger.Log(LogLevel.Debug, $"L'utilisateur a choisi {(choice ? "oui" : "non")}.");
            return choice;
        }

        /// <summary>
        /// Exits the installation.
        /// </summary>
        public static void Exit(int code = 0)
        {
            Logger.EndLogging();
            Environment.Exit(code);
        }
    }
}
