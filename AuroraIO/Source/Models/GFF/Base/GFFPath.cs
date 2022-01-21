using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Source.Models.GFF {
    public class GFFPath {

        private String internalPath;

        public String[] pathComponents {
            get {
                return internalPath.Split(new char[] { '\\', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public bool hasOneComponent {
            get {
                return pathComponents.Length == 1;
            }
        }

        public bool isEmpty {
            get {
                return pathComponents.Length == 0;
            }
        }

        public GFFPath first() {
            if (pathComponents.Length > 0) {
                return pathComponents.First();
            } else {
                return "";
            }       
        }

        public GFFPath last() {
            if (pathComponents.Length > 0) {
                return pathComponents.Last();
            } else {
                return "";
            }
        }

        public GFFPath removingFirst() {
            List<String> pathComponents = this.pathComponents.ToList();
            if (pathComponents.Count > 0) {
                pathComponents.RemoveAt(0);
            }
            
            return new GFFPath(String.Join("\\", pathComponents.ToArray()));
        }

        public GFFPath removingLast() {
            List<String> pathComponents = this.pathComponents.ToList();
            if (pathComponents.Count > 0) {
                pathComponents.RemoveAt(pathComponents.Count - 1);
                return new GFFPath(String.Join("\\", pathComponents.ToArray()));
            } else {
                return "";
            }
        }

        GFFPath(String internalPath) {
            this.internalPath = internalPath;
        }

        public static implicit operator GFFPath(string address) {
            return new GFFPath(address);
        }

        public static implicit operator String(GFFPath path) {
            return path.internalPath;
        }

        public static string operator +(String originalString, GFFPath path) {
            return originalString + "\\" + path.internalPath;
        }

        public static GFFPath operator +(GFFPath originalPath, String newPath) {
            return originalPath.internalPath + "\\" + newPath;
        }

        public static GFFPath operator +(GFFPath originalPath, GFFPath newPath) {
            return originalPath.internalPath + "\\" + newPath.internalPath;
        }

        public override bool Equals(object obj) {
            return internalPath.Equals(obj as String);
        }

        public override string ToString() {
            return internalPath;
        }
    }
}
