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
        /// Server empfängt Daten
        /// </summary>
        RECEIVE_DATA = 1,

        /// <summary>
        /// Server sendet Daten
        /// </summary>
        SEND_DATA = 2
    }
}
