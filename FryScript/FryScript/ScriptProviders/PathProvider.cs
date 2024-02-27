using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FryScript.ScriptProviders
{
    public static class PathProvider
    {
        public const char DirectorySeperator = '/';
        public const string RootDirectory = "~/";
        public const string DirectoryTraversal = "..";

        private static readonly Regex _fixPath = new(@"^(\/|\.\/|~\/)");
        private static readonly Regex _invalidCharacters = new(@"(\\|<|>|:|\""|\?|\*|\||\/{2,})");
        private static readonly Regex _fileExtension = new(@"\.[^\/]+$");
        private static readonly Regex _fileName = new(@"[^\/\.]+$");

        public static string EnsurePath(string path)
        {
            var invalidCharacters = _invalidCharacters.Match(path);

            if (invalidCharacters.Success)
                throw new ScriptPathException($"Path '{path}' contains invalid characters. Invalid character '{invalidCharacters.Value}' found at position {invalidCharacters.Index}.");

            var fileExtension = _fileExtension.Match(path);

            if(fileExtension.Success)
                throw new ScriptPathException($"Path '{path}' cannot contain a file extension. File extension '{fileExtension.Value}' found at position {fileExtension.Index}.");

            return FixPath(path);
        }

        public static string Resolve(string path)
        {
            var fixedPath = EnsurePath(path);

            if (!fixedPath.Contains(DirectoryTraversal))
                return fixedPath;

            var parts = fixedPath.Split(DirectorySeperator);

            var newParts = new List<string>();

            foreach (var part in parts)
            {
                if (part != DirectoryTraversal)
                {
                    newParts.Add(part);
                    continue;
                }

                if (newParts.Count == 0)
                    throw new ScriptPathException($"Unable to resolve path '{path}' as it traverses outside the root directory.");

                newParts.RemoveAt(newParts.Count - 1);
            }

            var virtualPath = string.Join(DirectorySeperator.ToString(), newParts);

            return virtualPath;
        }

        public static string Resolve(string relativeTo, string path)
        {
            if (path.StartsWith(RootDirectory))
                return Resolve(path);

            relativeTo = _fileName.Replace(relativeTo, string.Empty);
            relativeTo = EnsurePath(relativeTo).Trim(DirectorySeperator);
            path = EnsurePath(path);

            var rootPath = string.Concat(relativeTo, DirectorySeperator, path);

            rootPath = Resolve(rootPath);

            return rootPath;
        }

        public static string GetFileName(string path)
        {
            var fixedPath = EnsurePath(path);
            var file = _fileName.Match(fixedPath);

            if (!file.Success)
                throw new ScriptPathException($"Path '{path}' does not contain a file name.");

            return file.Value;
        }

        public static string FixPath(string path)
        {
            path = _fixPath.Replace(path, string.Empty);

            path.Trim();

            return path;
        }
    }
}
