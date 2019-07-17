/* Copyright (C) Marcel Bulla - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Marcel Bulla <postmaster@marcel-bulla.de>
 */

namespace De.Markellus.Maths.Core.Arithmetic
{
    /// <summary>
    /// Status des Spigot-Servers
    /// </summary>
    internal enum SpigotStatus
    {
        /// <summary>
        /// Server wird heruntergefahren
        /// </summary>
        EXIT = -1,

        /// <summary>
        /// Server sendet Daten
        /// </summary>
        RECEIVE_DATA = 100,

        /// <summary>
        /// Server sendet Daten, es folgen noch weitere Daten
        /// </summary>
        RECEIVE_DATA_PART = 101,

        /// <summary>
        /// Empfang der Daten vom Client bestätigt.
        /// </summary>
        RECEIVE_DATA_RECEIVED = 102,

        /// <summary>
        /// Server empfängt Daten
        /// </summary>
        SEND_DATA = 200,

        /// <summary>
        /// Server empfängt Daten, es folgen noch weitere Daten
        /// </summary>
        SEND_DATA_PART = 201,

        /// <summary>
        /// Empfang der Daten wurden vom Server bestätigt.
        /// </summary>
        SEND_DATA_RECEIVED = 202
    }
}
