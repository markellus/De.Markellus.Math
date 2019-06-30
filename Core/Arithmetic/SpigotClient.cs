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
        /// Grösse der Datensektion
        /// </summary>
        private const int MMF_SIZE_DATA = 4096;

        /// <summary>
        /// Status-Offset
        /// </summary>
        private const int MMF_OFFSET_STATUS = 0;

        /// <summary>
        /// Offset an dem gespeichert ist, wie gross der Datenspeicher des Servers ist
        /// </summary>
        private const int MMF_OFFSET_DATA_MAXSIZE = 4;

        /// <summary>
        /// Grösse der Daten, die derzeit im Datenspeicher des Servers liegen
        /// </summary>
        private const int MMF_OFFSET_DATA_SIZE = 8;

        /// <summary>
        /// Daten-Offset
        /// </summary>
        private const int MMF_OFFSET_DATA = 12;

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
        /// Die Grösse des Server-Datenspeichers.
        /// </summary>
        private static int _iMaxDataSize;

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
            _accessor = _mmf.CreateViewAccessor(MMF_OFFSET_DATA_MAXSIZE, sizeof(int));

            _iMaxDataSize = _accessor.ReadInt32(0);
            _accessor.Dispose();
            _accessor = _mmf.CreateViewAccessor(0, _iMaxDataSize + sizeof(int) * 3);

            _mutex.WaitOne();
        }

        /// <summary>
        /// Beendet die Verbindung mit dem Spigot-Server.
        /// </summary>
        public static void Stop()
        {
            _mutex.ReleaseMutex();

#if !DEBUG_SERVER
            //Server herunterfahren
            SendStatus(SpigotStatus.EXIT);
#endif
            //IPC-Verbindung trennen
            _accessor.Dispose();
            _mmf.Dispose();
        }

        /// <summary>
        /// Führt eine Addition durch.
        /// </summary>
        /// <param name="strRealLeft">Linker Operand</param>
        /// <param name="strRealRight">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static string Add(string strRealLeft, string strRealRight)
        {
            return ProcessData(strRealLeft + "+" + strRealRight);
        }

        /// <summary>
        /// Führt eine Subtraktion durch.
        /// </summary>
        /// <param name="strRealLeft">Linker Operand</param>
        /// <param name="strRealRight">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static string Subtract(string strRealLeft, string strRealRight)
        {
            return ProcessData(strRealLeft + "-" + strRealRight);
        }

        /// <summary>
        /// Führt eine Multiplikation durch.
        /// </summary>
        /// <param name="strRealLeft">Linker Operand</param>
        /// <param name="strRealRight">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static string Multiply(string strRealLeft, string strRealRight)
        {
            return ProcessData(strRealLeft + "*" + strRealRight);
        }

        /// <summary>
        /// Führt eine Division durch.
        /// </summary>
        /// <param name="strRealLeft">Linker Operand</param>
        /// <param name="strRealRight">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static string Divide(string strRealLeft, string strRealRight)
        {
            return ProcessData(strRealLeft + "/" + strRealRight);
        }

        /// <summary>
        /// Führt eine Exponentiation durch.
        /// </summary>
        /// <param name="strRealLeft">Linker Operand</param>
        /// <param name="strRealRight">Rechter Operand</param>
        /// <returns>Ergebnis der Operation</returns>
        public static string Pow(string strRealLeft, string strRealRight)
        {
            return ProcessData(strRealLeft + "^" + strRealRight);
        }

        private static string ProcessData(string strData)
        {
            SendStatus(SpigotStatus.SEND_DATA_RECEIVED);
            _mutex.ReleaseMutex();
            SendData(strData);
            SendStatus(SpigotStatus.RECEIVE_DATA_RECEIVED);
            string strResult = ReceiveData();
            _mutex.WaitOne();
            return strResult;
        }

        /// <summary>
        /// Sendet einen String mit Daten an den Server und befiehlt dem Server diese zu verarbeiten.
        /// </summary>
        /// <param name="strData">Die zu übertragenden Daten</param>
        private static void SendData(string strData)
        {
            byte[] arrEncoded = Encoding.ASCII.GetBytes("(" + strData + ")");
            int iPos = 0;

            while (iPos < arrEncoded.Length || ReceiveStatus() != SpigotStatus.SEND_DATA_RECEIVED)
            {
                _mutex.WaitOne();

                if (ReceiveStatus() == SpigotStatus.SEND_DATA_RECEIVED)
                {
                    if (arrEncoded.Length - iPos > _iMaxDataSize)
                    {
                        SendStatus(SpigotStatus.SEND_DATA_PART);
                        _accessor.Write(MMF_OFFSET_DATA_SIZE, _iMaxDataSize);
                        _accessor.WriteArray(MMF_OFFSET_DATA, arrEncoded, iPos, _iMaxDataSize);
                        _accessor.Flush();
                        iPos += _iMaxDataSize;
                    }
                    else if(arrEncoded.Length - iPos > 0)
                    {
                        SendStatus(SpigotStatus.SEND_DATA);
                        _accessor.Write(MMF_OFFSET_DATA_SIZE, arrEncoded.Length - iPos);
                        _accessor.WriteArray(MMF_OFFSET_DATA, arrEncoded, iPos, arrEncoded.Length - iPos);
                        _accessor.Flush();
                        iPos += arrEncoded.Length - iPos;
                    }
                }

                _mutex.ReleaseMutex();
            }

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
            _accessor.Write(MMF_OFFSET_STATUS, (int)status);
        }

        /// <summary>
        /// Empfängt einen String mit Daten vom Server.
        /// </summary>
        /// <returns>Der empfangene Daten-String</returns>
        private static string ReceiveData()
        {
            bool bReceived = false;
            StringBuilder builder = new StringBuilder();

            while (!bReceived)
            {
                _mutex.WaitOne();

                SpigotStatus status = ReceiveStatus();

                if (status == SpigotStatus.RECEIVE_DATA || status == SpigotStatus.RECEIVE_DATA_PART)
                {

                    int iSize = _accessor.ReadInt32(MMF_OFFSET_DATA_SIZE);
                    byte[] arrResult = new byte[iSize];
                    _accessor.ReadArray(MMF_OFFSET_DATA, arrResult, 0, iSize);
                    string strPart = Encoding.ASCII.GetString(arrResult).Split(new char[] { '\n' })[0];
                    builder.Append(strPart);
                    SendStatus(SpigotStatus.RECEIVE_DATA_RECEIVED);
                }

                if (status == SpigotStatus.RECEIVE_DATA)
                {
                    bReceived = true;
                }
                _mutex.ReleaseMutex();

                
            }

            string strResult = builder.ToString();

            if (strResult.StartsWith("ERROR"))
            {
                throw new InvalidDataException($"The server has thrown an error: {strResult}");
            }

#if DEBUG
            File.AppendAllText("spigot.log", "recv: " + strResult + "\n");
#endif

            return strResult;
        }

        /// <summary>
        /// Liesst den Server-Status aus.
        /// </summary>
        private static SpigotStatus ReceiveStatus()
        {
            return (SpigotStatus)_accessor.ReadInt32(MMF_OFFSET_STATUS);
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
