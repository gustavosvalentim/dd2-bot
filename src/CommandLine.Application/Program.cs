using CommandLine.Application.Services;
using Persistence.FolderStorage;

// Settings
string StoragePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DD2Bot";
string TemplatesPath = StoragePath + "\\templates";

// Dependencies
var imageRepository = new FolderStorageImageRepository();
var screenCaptureService = new ScreenCaptureService(imageRepository);
var templateMatchService = new TemplateMatchService(imageRepository);

// Mainloop
var buffer = screenCaptureService.CaptureScreen();
var templateMatch = await templateMatchService.MatchSource(buffer, TemplatesPath + "\\vs-debug-actions.png", 0.8f);
if (templateMatch != null)
{
    Console.WriteLine("Found a match");
    Console.WriteLine($"Template: {templateMatch.TemplateName}");
    Console.WriteLine($"Match percentage: {templateMatch.MatchPercentage}");
    Console.WriteLine($"X Position: {templateMatch.XPosition}");
    Console.WriteLine($"Y Position: {templateMatch.YPosition}");
}