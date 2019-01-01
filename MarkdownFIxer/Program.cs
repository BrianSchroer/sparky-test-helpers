using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MarkdownFixer
{
    public class Program
    {
        private static readonly Regex _localLinkRegex = 
            new Regex("href=\"(?!http)([^\"]*(?<!\\.md))\"", RegexOptions.Compiled);

        public static void Main(string[] args)
        {
            const string relativePath = "../../../SparkyTestHelpers.Populater/Help";

            string myDirectoryPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            string markdownDirectoryPath = ResolveRelativePath(myDirectoryPath, relativePath);

            FixMarkdownFiles(markdownDirectoryPath);
        }

        private static string ResolveRelativePath(string basePath, string relativePath)
        {
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
        }

        private static void FixMarkdownFiles(string directoryPath)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);

            if (!directoryInfo.Exists)
            {
                Console.WriteLine($"Directory not found: \"{directoryPath}\".");
            }

            FileInfo[] markdownFiles = directoryInfo.GetFiles("*.md", SearchOption.AllDirectories);

            foreach (FileInfo markdownFile in markdownFiles)
            {
                FixMarkdownFile(markdownFile);
            }
        }

        private static void FixMarkdownFile(FileInfo markdownFile)
        {
            string fileContents = "";
            string filePath = markdownFile.FullName;

            using (var sr = new StreamReader(filePath))
            {
                fileContents = sr.ReadToEnd();
            }

            int matchCount = _localLinkRegex.Matches(fileContents).Count;
            if (matchCount == 0) return;

            Console.WriteLine($"Updating {matchCount} local link(s) in {filePath}...");

            string updatedFileContents = _localLinkRegex.Replace(fileContents, $"href=\"$1.md\"");

            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(updatedFileContents);
            }
        }
    }
}
