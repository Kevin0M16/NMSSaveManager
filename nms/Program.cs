using System;
using NMSSaveManager;

namespace nms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("encrypt or decrypt?: ");
            string choice = Console.ReadLine();

            if (choice == "decrypt")
            {
                Console.WriteLine("Enter hgFilePath (source): ");
                string hgFilePath = @".\" + Console.ReadLine();
                Console.WriteLine("Enter jsonOutput Path (destination): ");
                string jsonOutputPath = @".\" + Console.ReadLine();

                GameSave.DecryptSave(hgFilePath, jsonOutputPath);
            }

            if (choice == "encrypt")
            {
                Console.WriteLine("Enter saveslot: ");
                uint saveslot = Convert.ToUInt32(Console.ReadLine());
                Console.WriteLine("Enter hgFilePath (destination): ");
                string hgFilePath = @".\" + Console.ReadLine();
                Console.WriteLine("Enter jsonInput Path (source): ");
                string jsonInputPath = @".\" + Console.ReadLine();

                GameSave.EncryptSave(saveslot, hgFilePath, jsonInputPath);
            }
            
        }
    }
}
