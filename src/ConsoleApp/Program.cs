using DD2Bot.ApplicationCore;
using DD2Bot.ConsoleApp.Services;
using DD2Bot.Adapters.FileSystemStorage;
using DD2Bot.Adapters.WindowsScreenCapture;

// Settings
string storagePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DD2Bot";
string templatesPath = storagePath + "\\templates";

if (args.Length > 0 && args[0] == "settings:get")
{
    Console.WriteLine($"Storage path: {storagePath}");
    Console.WriteLine($"Templates path: {templatesPath}");
    return;
}

// Dependencies
var fileRepository = new FileSystemFileRepository();
var screenCaptureService = new WindowsScreenCaptureService();
var templateMatchService = new TemplateMatchService(fileRepository);

// Mainloop
var vsDebugActions = new Template("VS Debug Actions", templatesPath + "\\vs-debug-actions.png");
var buffer = screenCaptureService.CaptureDesktop();
var templateMatch = await templateMatchService.Match(buffer, vsDebugActions, 0.8f);
if (templateMatch != null)
{
    Console.WriteLine("Found a match");
    Console.WriteLine($"Template: {templateMatch.Template.Name}");
    Console.WriteLine($"Match percentage: {templateMatch.MatchPercentage}");
    Console.WriteLine($"X Position: {templateMatch.XPosition}");
    Console.WriteLine($"Y Position: {templateMatch.YPosition}");
}