using System.IO;
using System.Linq;

namespace NzbDrone.Mono.EnvironmentInfo
{
    public class LinuxVersionInfo
    {
        public LinuxVersionInfo()
        {
            var releaseFiles = Directory.GetFiles("/etc/", "*-release").ToList();
            releaseFiles.ForEach(ImportValues);
        }

        private void ImportValues(string releaseFile)
        {
            var lines = File.ReadAllLines(releaseFile);

            foreach (var line in lines)
            {
                var parts = line.Split('=');
                if (parts.Length >= 2)
                {
                    var key = parts[0];
                    var value = parts[1];

                    if (!string.IsNullOrWhiteSpace(value))

                    {
                        switch (key)
                        {
                            case "ID":
                                Name = value;
                                break;
                            case "PRETTY_NAME":
                                FullName = value;
                                break;
                            case "VERSION_ID":
                                Version = value;
                                break;

                        }
                    }
                }

            }
        }

        public string Version { get; private set; }

        public string Name { get; private set; }

        public string FullName { get; private set; }
    }
}