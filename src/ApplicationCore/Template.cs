namespace DD2Bot.ApplicationCore
{
    public class Template
    {
        public string Name { get; }
        public string ImagePath { get; }

        public Template(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
        }
    }
}
