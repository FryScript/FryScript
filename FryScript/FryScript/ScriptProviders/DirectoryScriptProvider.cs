using System;
using System.IO;

namespace FryScript.ScriptProviders
{
    public class DirectoryScriptProvider : IScriptProvider
    {
        private readonly DirectoryInfo _root;

        public DirectoryScriptProvider(string path)
        {
            _root = new DirectoryInfo(path ?? throw new ArgumentNullException(nameof(path)));

            if (!_root.Exists)
                throw new ArgumentException($"Directory path {path} does not exist", nameof(path));
        }

        public string GetScript(Uri uri)
        {
            uri = uri ?? throw new ArgumentNullException(nameof(uri));

            if (!uri.IsAbsoluteUri)
                throw new ArgumentException("Uri is not absolute", nameof(uri));

            var file = new FileInfo(uri.LocalPath);

            if (!file.Exists)
                throw new ArgumentException($"File at {uri.LocalPath} does not exist");

            return File.ReadAllText(uri.LocalPath);
        }

        public bool TryGetUri(string path, out Uri uri, string relativeTo = null)
        {
            path = path ?? throw new ArgumentNullException(nameof(path));

            uri = null;

            var searchDir = GetSearchDirctory(relativeTo);

            while (searchDir != null)
            {
                if (TryGetFile(searchDir, path, out uri))
                    return true;

                searchDir = searchDir.Parent;
            }

            return false;
        }

        private DirectoryInfo GetSearchDirctory(string relativeTo)
        {
            if (relativeTo == null)
                return _root;

            var uri = new Uri(Uri.UnescapeDataString(relativeTo));

            if (!uri.IsFile)
                return null;

            var relativeFile = new FileInfo(Path.GetFullPath(uri.LocalPath));

            if (!relativeFile.Exists)
                return null;

            return relativeFile.Directory;
        }

        private bool TryGetFile(DirectoryInfo searchDir, string path, out Uri uri)
        {
            uri = null;

            var filePath = Path.Combine(searchDir.FullName, path);
            var file = new FileInfo(filePath);

            if (!file.Exists)
                return false;

            if(!file.FullName.StartsWith(_root.FullName, StringComparison.OrdinalIgnoreCase))
                return false;

            uri = new Uri(file.FullName);

            return true;
        }
    }
}
