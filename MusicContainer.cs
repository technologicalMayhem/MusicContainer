using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MusicContainer
{
    public class MusicContainer
    {
        //Usually the name of an album
        public string Name { get; set; }
        public string[] Files { get; set; }
        public string[] Tags { get; set; }

        public static MusicContainer Open(string path)
        {
            throw new NotImplementedException();
        }

        public static void CreateContainer(string pathToFoler, string fileName)
        {
            var files = Directory.GetFiles(pathToFoler).ToDictionary(x => x, x => new FileInfo(x).Length);
            using (var writer = new FileStream(fileName + ".muc", FileMode.Create))
            {
                byte[] dict = Encoding.Unicode.GetBytes(String.Join(",", files.Select(x => String.Format("{0},{1}", x.Key, x.Value))));
                writer.Write(BitConverter.GetBytes(dict.Length), 0, 4); //The first 4 bytes contain the length of the file dictionary
                writer.Write(dict, 0, dict.Length);
                foreach (var entry in files)
                {
                    byte[] file = File.ReadAllBytes(entry.Key);
                    writer.Write(file, 0, file.Length);
                }
            }
        }

        public static void ExtractContainer(string pathToFile, string outputPath)
        {
            var read = new FileStream("", FileMode.Open);
            using (var reader = new StreamReader(pathToFile))
            {
                var rawFiles = reader.ReadLine().Split(',');
                var files = new Dictionary<string, long>();
                for (int i = 0; i < (rawFiles.Length / 2); i += 2)
                {
                    files.Add(rawFiles[i], long.Parse(rawFiles[i + 1]));
                }
            }
        }
    }
}
