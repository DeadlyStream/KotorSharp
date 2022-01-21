
using KPatcher.Source.Managers;
using System;

namespace KPatcher {
    class Program {
        static void Main(string[] args) {
            ProgramController manager = new ProgramController();
            manager.acceptArgs(args);
            Console.ReadLine();
        }
    }
}
