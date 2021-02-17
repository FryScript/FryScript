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

        public bool TryGetScriptInfo(string path, out ScriptInfo scriptInfo, Uri relativeTo = null)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));

            scriptInfo = null;

            var searchDir = GetSearchDirectory(relativeTo);

            while (searchDir != null)
            {
                if (TryGetScriptInfo(searchDir, path, out scriptInfo))
                    return true;

                searchDir = searchDir.Parent;
            }

            return false;
        }

        private DirectoryInfo GetSearchDirectory(Uri relativeTo)
        {
            if (relativeTo == null)
                return _root;

            if (!relativeTo.IsFile)
                return null;

            var relativeFile = new FileInfo(Path.GetFullPath(relativeTo.LocalPath));

            if (!relativeFile.Exists)
                return null;

            return relativeFile.Directory;
        }

        private bool TryGetScriptInfo(DirectoryInfo searchDir, string path, out ScriptInfo scriptInfo)
        {
            scriptInfo = null;

            var filePath = Path.Combine(searchDir.FullName, path);
            var file = new FileInfo(filePath);

            if (!file.Exists)
                return false;

            if(!file.FullName.StartsWith(_root.FullName, StringComparison.OrdinalIgnoreCase))
                return false;

            var uri = new Uri(file.FullName);

            scriptInfo = new ScriptInfo
            {
                Uri = uri,
                Source = File.ReadAllText(uri.LocalPath)
            };

            return true;
        }
    }
}
