using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace De.Markellus.Maths.Core.Arithmetic
{
    public static class SpigotApi
    {
        private static Dictionary<SpigotClient, bool> _dicClients;
        private static Queue<ManualResetEvent> _queueWaitingThreads;

        private static object _lockQueue;

        static SpigotApi()
        {
            //Der Garbage-Collector weigert sich bei Unit-Tests Dispose() aufzurufen...
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            _lockQueue = new object();
            foreach (Process p in Process.GetProcessesByName(SpigotClient.SERVER_NAME))
            {
                p.Kill();
            }

            _dicClients = new Dictionary<SpigotClient, bool>();
            _queueWaitingThreads = new Queue<ManualResetEvent>();

            for (int i = 0; i < Environment.ProcessorCount / 2; i++)
            {
                _dicClients.Add(new SpigotClient(), false);
            }
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

        public static string Mod(string strRealLeft, string strRealRight)
        {
            return ProcessData(strRealLeft + "%" + strRealRight);
        }

        public static string Root(string strRealLeft, string strRealRight)
        {
            return Pow(strRealLeft, "(1/" + strRealRight + ")");
        }

        private static SpigotClient LockClient()
        {
            lock (_lockQueue)
            {
                foreach (KeyValuePair<SpigotClient, bool> kvp in _dicClients)
                {
                    if (!kvp.Value)
                    {
                        _dicClients[kvp.Key] = true;
                        return kvp.Key;
                    }
                }

                SpigotClient client = new SpigotClient();
                _dicClients.Add(client, true);
                return client;
            }
        }

        private static void ReleaseClient(SpigotClient client)
        {
            lock (_lockQueue)
            {
                _dicClients[client] = false;

                //WORKAROUND: Memory-Leak in Spigot
                //client.Dispose();
                //_dicClients.Remove(client);
            }
        }

        private static string ProcessData(string strData)
        {
            SpigotClient client = LockClient();
            string strResult = client.ProcessData(strData);
            ReleaseClient(client);
            return strResult;
        }

        private static void OnProcessExit(object sender, EventArgs e)
        {
            foreach (SpigotClient client in _dicClients.Keys)
            {
                client.Dispose();
            }
        }
    }
}
