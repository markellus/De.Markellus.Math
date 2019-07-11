/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
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
    public class SpigotClient : IDisposable
    {
        public const string SERVER_NAME = "spgt-server";
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

        private static int _iClientCount = 0;

        /// <summary>
        /// Verweis auf den Server-Process
        /// </summary>
        private Process _procSpigot;

        /// <summary>
        /// Der Shared Mutex des Servers
        /// </summary>
        private Mutex _mutex;

        /// <summary>
        /// Der Shared Memory
        /// </summary>
        private MemoryMappedFile _mmf;

        /// <summary>
        /// Erlaubt Zugriff auf den SHared memory des Spigot-Servers
        /// </summary>
        private MemoryMappedViewAccessor _accessor;

        /// <summary>
        /// Die Grösse des Server-Datenspeichers.
        /// </summary>
        private int _iMaxDataSize;

        private readonly object _locker = new object();

        public SpigotClient()
        {
            string strServer = Path.GetFullPath(SERVER_LOCATION);
            string strMutex = "spgtmut" + _iClientCount;
            string strMmf = "spgtmmf" + _iClientCount++;

            //Server starten
            _procSpigot = new Process();
            //_procSpigot.StartInfo.UseShellExecute = true;
            _procSpigot.StartInfo.FileName = strServer;
            _procSpigot.StartInfo.Arguments += strMutex + " " + strMmf;
            _procSpigot.Start();

            //IPC-Verbindung herstellen
            bool bConnected = false;
            while (!bConnected)
            {
                //Darauf warten das der Server hochfährt
                while (!Mutex.TryOpenExisting(strMutex, out _mutex))
                {
                    Thread.Sleep(1);
                }

                try
                {
                    _mmf = MemoryMappedFile.OpenExisting(strMmf);
                    _accessor = _mmf.CreateViewAccessor(MMF_OFFSET_DATA_MAXSIZE, sizeof(int));
                    bConnected = true;
                }
                catch
                {
                    Console.WriteLine("IPC ERROR");
                    bConnected = false;
                }
            }

            _iMaxDataSize = _accessor.ReadInt32(0);
            _accessor.Dispose();
            _accessor = _mmf.CreateViewAccessor(0, _iMaxDataSize + sizeof(int) * 3);
        }

        public string ProcessData(string strData)
        {
            lock (_locker)
            {
                SendStatus(SpigotStatus.SEND_DATA_RECEIVED);
                SendData(strData);
                SendStatus(SpigotStatus.RECEIVE_DATA_RECEIVED);
                string strResult = ReceiveData();
                return strResult;
            }
        }

        /// <summary>
        /// Sendet einen String mit Daten an den Server und befiehlt dem Server diese zu verarbeiten.
        /// </summary>
        /// <param name="strData">Die zu übertragenden Daten</param>
        private void SendData(string strData)
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

                if (_procSpigot.HasExited)
                {
                    throw new ApplicationException("Spigot server has crashed");
                }
            }
        }

        /// <summary>
        /// Sendet eine Statusmeldung an den Server.
        /// </summary>
        /// <param name="status">Der zu übertragende Status.</param>
        private void SendStatus(SpigotStatus status)
        {
            _mutex.WaitOne();
            _accessor.Write(MMF_OFFSET_STATUS, (int)status);
            _mutex.ReleaseMutex();
        }

        /// <summary>
        /// Empfängt einen String mit Daten vom Server.
        /// </summary>
        /// <returns>Der empfangene Daten-String</returns>
        private string ReceiveData()
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

                if (_procSpigot.HasExited)
                {
                    throw new ApplicationException("Spigot server has crashed");
                }
            }

            string strResult = builder.ToString();

            if (strResult.StartsWith("ERROR"))
            {
                throw new InvalidDataException($"The server has thrown an error: {strResult}");
            }

            return strResult;
        }

        /// <summary>
        /// Liesst den Server-Status aus.
        /// </summary>
        private SpigotStatus ReceiveStatus()
        {
            return (SpigotStatus)_accessor.ReadInt32(MMF_OFFSET_STATUS);
        }

        public void Dispose()
        {
            //Server herunterfahren
            SendStatus(SpigotStatus.EXIT);
            _procSpigot.WaitForExit();

            //IPC-Verbindung trennen
            _accessor.Dispose();
            _mmf.Dispose();
            _mutex.Close();
        }
    }
}
