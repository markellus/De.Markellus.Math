/* 
 * This file is part of De.Markellus.Math (https://github.com/markellus/De.Markellus.Math).
 * Copyright (c) 2019 Marcel Bulla.
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace De.Markellus.Maths.Core.Arithmetic
{
    /// <summary>
    /// IPC-Client für den Spigot-Server
    /// </summary>
    public static class SpigotClient
    {
        /// <summary>
        /// Name des Shared Memory
        /// </summary>
        private const string MMF_NAME = "SPGT_SM";

        /// <summary>
        /// Name des Shared Mutex
        /// </summary>
        private const string MMF_MUTEX = "SPGT_SM_MTX";

        /// <summary>
        /// Grösse des Shared Memory
        /// </summary>
        private const int MMF_SIZE = 4096 + 4 + 4;

        /// <summary>
        /// Grösse der Datensektion
        /// </summary>
        private const int MMF_SIZE_DATA = 4096;

        /// <summary>
        /// Status-Offset
        /// </summary>
        private const int MMF_OFFSET_STATUS = 0;

        /// <summary>
        /// Argument-Offset
        /// </summary>
        private const int MMF_OFFSET_ARGSIZE = 4;

        /// <summary>
        /// Daten-Offset
        /// </summary>
        private const int MMF_OFFSET_DATA = 8;

        /// <summary>
        /// Ort der ausführbaren Server-Datei
        /// </summary>
        private const string SERVER_LOCATION = "./spgt/spgt-server.exe";

        /// <summary>
        /// Verweis auf den Server-Process
        /// </summary>
        private static Process _procSpigot;

        /// <summary>
        /// Der Shared Mutex des Servers
        /// </summary>
        private static Mutex _mutex;

        /// <summary>
        /// Der Shared Memory
        /// </summary>
        private static MemoryMappedFile _mmf;

        /// <summary>
        /// Erlaubt Zugriff auf den SHared memory des Spigot-Servers
        /// </summary>
        private static MemoryMappedViewAccessor _accessor;

        /// <summary>
        /// Verbindet den Client mit einem Spigot-Server oder startet einen Neuen, falls keiner vorhanden ist.
        /// </summary>
        public static void Start()
        {
#if DEBUG
            if (File.Exists("spigot.log"))
            {
                File.Delete("spigot.log");
            }
#endif

#if !DEBUG_SERVER
            //Checken, ob der Server schon läuft
            if (!Mutex.TryOpenExisting(MMF_MUTEX, out _mutex))
            {
                string strServer = Path.GetFullPath(SERVER_LOCATION);

                //checken, ob Server existiert
                if (!File.Exists(strServer))
                {
                    throw new FileNotFoundException(
                        "The spigot server is missing from this installation. Please try to reinstall the program.");
                }

                _procSpigot = new Process();
                _procSpigot.StartInfo.FileName = strServer;
                _procSpigot.Start();
            }
            //Existierenden Server übernehmen
            else
            {
                _procSpigot = Process.GetProcessesByName("spgt-server")[0];
            }
#endif
            //IPC-Verbindung herstellen
            if (_mutex == null)
            {
                //Darauf warten das der Server hochfährt
                while (!Mutex.TryOpenExisting(MMF_MUTEX, out _mutex))
                {
                    Thread.Sleep(1);
                }
            }

            _mmf = MemoryMappedFile.OpenExisting(MMF_NAME);
            _accessor = _mmf.CreateViewAccessor(0, MMF_SIZE);
        }

        /// <summary>
        /// Beendet die Verbindung mit dem Spigot-Server.
        /// </summary>
        public static void Stop()
        {
#if !DEBUG_SERVER
            //Server herunterfahren
            SendStatus(SpigotStatus.EXIT);
#endif
            //IPC-Verbindung trennen
            _accessor.Dispose();
            _mmf.Dispose();
        }

        /// <summary>
        /// Führt eine Addition aus.
        /// </summary>
        /// <param name="strRealLeft">Linker Operand</param>
        /// <param name="strRealRight">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static string Add(string strRealLeft, string strRealRight)
        {
            SendData(strRealLeft + "+" + strRealRight);
            return ReceiveData();
        }

        /// <summary>
        /// Führt eine Subtraktion aus.
        /// </summary>
        /// <param name="strRealLeft">Linker Operand</param>
        /// <param name="strRealRight">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static string Subtract(string strRealLeft, string strRealRight)
        {
            SendData(strRealLeft + "-" + strRealRight);
            return ReceiveData();
        }

        /// <summary>
        /// Sendet einen String mit Daten an den Server und befiehlt dem Server diese zu verarbeiten.
        /// </summary>
        /// <param name="strData">Die zu übertragenden Daten</param>
        private static void SendData(string strData)
        {
            //TODO checken ob client läuft
            //TODO checken ob Puffer überläuft
            _mutex.WaitOne();
            byte[] arrEncoded = Encoding.ASCII.GetBytes("(" + strData + ")");
            _accessor.Write(MMF_OFFSET_STATUS, (int)SpigotStatus.SEND_DATA);
            _accessor.Write(MMF_OFFSET_ARGSIZE, (int)1);
            _accessor.WriteArray(MMF_OFFSET_DATA, arrEncoded, 0, arrEncoded.Length);
            _accessor.Write(MMF_OFFSET_DATA + arrEncoded.Length, '\0');
            _accessor.Flush();
            _mutex.ReleaseMutex();

#if DEBUG
            File.AppendAllText("spigot.log","send: " + strData + "\n");
#endif
        }

        /// <summary>
        /// Sendet eine Statusmeldung an den Server.
        /// </summary>
        /// <param name="status">Der zu übertragende Status.</param>
        private static void SendStatus(SpigotStatus status)
        {
            _mutex.WaitOne();
            _accessor.Write(MMF_OFFSET_STATUS, (int)status);
            _mutex.ReleaseMutex();
        }

        /// <summary>
        /// Empfängt einen String mit Daten vom Server.
        /// </summary>
        /// <returns>Der empfangene Daten-String</returns>
        private static string ReceiveData()
        {
            int iCode = 0;
            while (iCode != (int)SpigotStatus.RECEIVE_DATA)
            {
                _mutex.WaitOne();
                iCode = _accessor.ReadInt32(MMF_OFFSET_STATUS);
                _mutex.ReleaseMutex();
            }
            
            byte[] arrResult = new byte[MMF_SIZE_DATA];

            _mutex.WaitOne();
            _accessor.ReadArray(MMF_OFFSET_DATA, arrResult, 0, MMF_SIZE_DATA);
            _mutex.ReleaseMutex();

            string strResult = Encoding.ASCII.GetString(arrResult).Split(new char[] { '\n' })[0];

            if (strResult.StartsWith("ERROR"))
            {
                throw new InvalidDataException($"The server has thrown an error: {strResult}");
            }

#if DEBUG
            File.AppendAllText("spigot.log", "recv: " + strResult + "\n");
#endif

            return strResult;
        }

        public static void Test()
        {
            if (Add("5", "5") != "10")
            {
                throw new SystemException("SpigotClient::Test 1");
            }

            if (Add("50", "5") != "55")
            {
                throw new SystemException("SpigotClient::Test 2");
            }

            if (Add("6436743", "54336436") != "60773179")
            {
                throw new SystemException("SpigotClient::Test 3");
            }

            if (Add("643674578938645390345667.746534736554765436258356324563253",
                    "436725467986784358754689354895709284309024092.3274198471090931238752896350935928703293285093258") !=
                "436725467986784358755333029474647929699369760.0739545836638585601336459596568458703293285093258")
            {
                throw new SystemException("SpigotClient::Test 4");
            }

            if (Add("-5", "5") != "0")
            {
                throw new SystemException("SpigotClient::Test 5");
            }

            if (Add("-5.0", "5.0") != "0")
            {
                throw new SystemException("SpigotClient::Test 6");
            }

            if (Subtract("6", "5") != "1")
            {
                throw new SystemException("SpigotClient::Test 7");
            }
        }
    }
}
