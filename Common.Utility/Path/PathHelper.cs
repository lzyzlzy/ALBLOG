using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.IO
{
    public static class PathHelper
    {
        public static List<string> FindFileByExtension(string path, bool onlyCurrentDir = false, params string[] extensions)
        {
            return FindFileByExtension(path, extensions.AsEnumerable(), onlyCurrentDir);
        }

        public static List<string> FindFileByExtension(string path, IEnumerable<string> extensions, bool onlyCurrentDir = false)
        {
            List<string> pathlist = new List<string>();
            if (!onlyCurrentDir)
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    pathlist.AddRange(FindFileByExtension(dir, extensions));
                }
            }
            pathlist.AddRange(Directory.GetFiles(path).Where(i => IsTrueFormat(i, extensions)));
            return pathlist;
        }

        public static bool IsTrueFormat(string path, IEnumerable<string> extensions)
        {
            foreach (var extension in extensions)
            {
                if (path.EndsWith(extension))
                    return true;
            }
            return false;
        }
    }
}