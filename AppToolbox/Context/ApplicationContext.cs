using System;
using System.Collections.Generic;
using AppToolbox.Extensions;
using AppToolbox.Source.Managers;

namespace AppToolbox.Context {
    public sealed class ApplicationContext {
        private static readonly ApplicationContext instance = new ApplicationContext();

        private Dictionary<String, ApplicationContextItemType> objectMap = new Dictionary<string, ApplicationContextItemType>();

        public readonly FileLoader fileLoader = new FileLoader();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ApplicationContext() {
        }

        private ApplicationContext() {
        }

        public static ApplicationContext Shared {
            get {
                return instance;
            }
        }

        public T getManager<T>() where T: ApplicationContextItemType {
            ApplicationContextItemType item = objectMap.safeGet(typeof(T).Name);
            setManager(item);
            return (T)item;
        }

        public void setManager<T>(T item) where T: ApplicationContextItemType {
            objectMap[typeof(T).Name] = item;
        }
    }
}
