using System;
using System.Collections.Generic;
using DupFinder.Domain.Enums;

namespace DupFinder.Domain
{
    public class Configuration
    {
        public List<string> Directories { get; set; } = new List<string>();
        public DetectionMode Mode { get; set; } = DetectionMode.File;
        public OutputMode OutputMode { get; set; } = OutputMode.Console;
        public string OutputTarget { get; set; }
        public bool IncludeEmpty { get; set; } = false;
        public bool ShowUsage { get; set; } = true;

        public static Configuration Build(string[] args)
        {
            var config = new Configuration();

            if (args == null || args.Length == 0)
            {
                return config;
            }

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToUpper())
                {
                    case "-M":
                    case "--MODE":
                        if (i + 1 >= args.Length)
                        {
                            throw new ArgumentException($"Missing argument for {args[i]}");
                        }

                        if (!Enum.TryParse(args[++i], ignoreCase: true, out DetectionMode detectionMode))
                        {
                            throw new ArgumentException($"Invalid value for {args[i]}");
                        }

                        config.Mode = detectionMode;
                        break;

                    case "-O":
                    case "--OUTPUT":
                        if (i + 1 >= args.Length)
                        {
                            throw new ArgumentException($"Missing argument for {args[i]}");
                        }

                        config.OutputTarget = args[++i];
                        break;

                    case "-OM":
                    case "--OUTPUT-MODE":
                        if (i + 1 >= args.Length)
                        {
                            throw new ArgumentException($"Missing argument for {args[i]}");
                        }

                        if (!Enum.TryParse(args[++i], ignoreCase: true, out OutputMode outputMode))
                        {
                            throw new ArgumentException($"Invalid value for {args[i]}");
                        }

                        config.OutputMode = outputMode;
                        break;

                    case "-I":
                    case "--INCLUDE-EMPTY":
                        config.IncludeEmpty = true;
                        break;
                    default:
                        config.Directories.Add(args[i]);
                        break;
                }
            }

            if (config.Directories.Count == 0)
            {
                config.Directories.Add(".");
            }

            config.ShowUsage = false;

            return config;
        }
    }
}
