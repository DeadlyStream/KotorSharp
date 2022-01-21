using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppToolbox.Classes {
    public class ApplicationPath {

        public String directoryName {
            get {
                return pathComponent.Split(new char[] { Path.DirectorySeparatorChar }).Last();
            }
        }

        public ApplicationPath[] subDirectories(string searchPattern, bool includeAll) {
            SearchOption searchOption = includeAll ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.GetDirectories(pathComponent, searchPattern, searchOption).Select(path => new ApplicationPath(path)).ToArray();
        }

        public ApplicationPath[] files(string searchPattern, bool includeAll) {
            SearchOption searchOption = includeAll ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.GetFiles(pathComponent, searchPattern, searchOption).Select(path => new ApplicationPath(path)).ToArray();
        }

        public ApplicationPath parentDirectory {
            get {
                return Path.GetDirectoryName(pathComponent);
            }
        }

        public string[] pathComponents {
            get {
                return pathComponent.Split(Path.DirectorySeparatorChar);
            } set {
                pathComponent = String.Join(Path.DirectorySeparatorChar.ToString(), value);
            }
        }

        public string pathComponent { get; protected set; }

        protected ApplicationPath(string pathComponent) {
            this.pathComponent = pathComponent.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }

        public static implicit operator ApplicationPath(string address) {
            return new ApplicationPath(address);
        }

        public static implicit operator String(ApplicationPath path) {
            return path.pathComponent;
        }

        public static string operator +(String originalString, ApplicationPath path) {
            return originalString + path.pathComponent;
        }

        public static ApplicationPath operator +(ApplicationPath originalPath, String newPath) {
            return originalPath.pathComponent + Path.DirectorySeparatorChar + newPath;
        }

        public static ApplicationPath operator +(ApplicationPath originalPath, ApplicationPath newPath) {
            return originalPath.pathComponent + Path.DirectorySeparatorChar + newPath.pathComponent;
        }

        public override string ToString() {
            return Path.GetFullPath(pathComponent);
        }
    }

    public static class ApplicationPathExtensions {
        public static ApplicationPath[] subDirectories(this ApplicationPath kotorPath) {
            return kotorPath.subDirectories("*");
        }

        public static ApplicationPath[] subDirectories(this ApplicationPath kotorPath, string searchPattern) {
            return kotorPath.subDirectories(searchPattern, false);
        }

        public static ApplicationPath[] subDirectories(this ApplicationPath kotorPath, bool includeAll) {
            return kotorPath.subDirectories("*", includeAll);
        }

        public static ApplicationPath[] files(this ApplicationPath kotorPath) {
            return kotorPath.files("*");
        }

        public static ApplicationPath[] files(this ApplicationPath kotorPath, bool includeAll) {
            return kotorPath.files("*", includeAll);
        }

        public static ApplicationPath[] files(this ApplicationPath kotorPath, string searchPattern) {
            return kotorPath.files(searchPattern, false);
        }
    }
}
