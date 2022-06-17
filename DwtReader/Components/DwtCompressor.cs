//-----------------------------------------------------------------------------
// <copyright file="DwtCompressor.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для работы с распаковкой данных при помощи алгоритма LZ77.
// </summary>
//-----------------------------------------------------------------------------

using Usl;

namespace DwtReader.Components
{
    /// <summary>
    /// Распаковщик данных при помощи алгоритма LZ77.
    /// </summary>
    internal class DwtCompressor
    {
        /// <summary>
        /// Распаковывает сжатые данные.
        /// </summary>
        /// <param name="data">Сжатые данные</param>
        /// <param name="size">Размер распаковываемых данных</param>
        /// <returns>Распакованные данные</returns>
        public byte[] Decompress(byte[] data, uint size)
        {
            Logger.Info(string.Format("Распаковка последних 2х байт: 0x{0:X2} 0x{1:X2}", data[data.Length - 2], data[data.Length - 1]));

            uint position         = default;
            uint resultPosition   = default;
            byte[] decompressData = new byte[size];

            uint literalCount = LiteralLength(data, ref position);

            for (var i = 0; i < literalCount; ++i)
                decompressData[resultPosition++] = data[position++];

            uint compressBytes;
            uint compressOffset;

            while (position < data.Length && (resultPosition < size + 1)) {
                byte opcode = data[position++];
                if (opcode == 0x10) {
                    compressBytes = LongCompressionOffset(data, ref position) + 9;
                    compressOffset = TwoByteOffset(data, ref position, ref literalCount) + 0x3FFF;
                    if (literalCount == 0)
                        literalCount = LiteralLength(data, ref position);
                }
                else if (opcode > 0x11 && opcode < 0x20) {
                    compressBytes = (uint)((opcode & 0x0F) + 2);
                    compressOffset = TwoByteOffset(data, ref position, ref literalCount) + 0x3FFF;
                    if (literalCount == 0)
                        literalCount = LiteralLength(data, ref position);
                }
                else if (opcode == 0x20) {
                    compressBytes = LongCompressionOffset(data, ref position) + 0x21;
                    compressOffset = TwoByteOffset(data, ref position, ref literalCount);
                    if (literalCount == 0)
                        literalCount = LiteralLength(data, ref position);
                    else
                        opcode = 0x00;
                }
                else if (opcode > 0x20 && opcode < 0x40) {
                    compressBytes = (uint)(opcode - 0x1E);
                    compressOffset = TwoByteOffset(data, ref position, ref literalCount);
                    if (literalCount == 0)
                        literalCount = LiteralLength(data, ref position);
                }
                else if (opcode > 0x3F) {
                    compressBytes = (uint)(((opcode & 0xF0) >> 4) - 1);
                    byte literalLehgth = data[position++];
                    compressOffset = (uint)((literalLehgth << 2) | ((opcode & 0x0C) >> 2));
                    literalCount = (uint)(opcode & 0x03);
                    if (literalCount < 1)
                        literalCount = LiteralLength(data, ref position);
                }
                else if (opcode == 0x11) {
                    Logger.Trace($"Распаковка завершена: {position}/{resultPosition}");
                    return decompressData;
                }
                else { // literalLehgth < 0x10
                    Logger.Warn($"Распаковка завершилась с неизвестным символом: {position}/{resultPosition}");
                    return decompressData;
                }

                uint remaining = size - (literalCount + resultPosition);
                if (remaining < compressBytes) {
                    compressBytes = remaining;
                    Logger.Warn($"Распаковка завершилась в выходом за пределы: {position}/{resultPosition}");
                }

                for (uint i = 0, j = resultPosition - compressOffset - 1; i < compressBytes; i++)
                    decompressData[resultPosition++] = decompressData[j++];

                for (var i = 0; i < literalCount; i++)
                    decompressData[resultPosition++] = data[position++];
            }
            Logger.Warn($"Распаковка не завершилась: {position}/{resultPosition} ");
            return decompressData;
        }

        /// <summary>
        /// Расшифровка с 32-байтового заголовка раздела.
        /// </summary>
        /// <param name="buf">Заголовок раздела</param>
        /// <param name="offset">Смещение</param>
        unsafe public byte[] Decrypt18(byte[] buf, uint offset)
        {
            byte size = (byte)(buf.Length / 4);
            uint mask = 0x4164536B ^ offset;
            fixed (byte* p = &buf[0]) {
                uint* pHdr = (uint*)p;
                for (var i = 0; i < size; ++i)
                    *pHdr++ ^= mask;
            }
            return buf;
        }

        /// <summary>
        /// Считает длину литерала.
        /// </summary>
        /// <param name="data">Массив с данными</param>
        /// <param name="position">Позиция в массиве</param>
        /// <returns>Длина литерала</returns>
        private uint LiteralLength(byte[] data, ref uint position)
        {
            uint cont = 0;
            byte literalLength = data[position++];

            if (literalLength > 0x0F) {
                position--;
                return 0;
            }

            if (literalLength == 0x00) {
                cont = 0x0F;
                literalLength = data[position++];
                while (literalLength == 0x00) {
                    cont += 0xFF;
                    literalLength = data[position++];
                }
            }
            cont += literalLength;
            cont += 3; //already sum 3
            return cont;
        }

        /// <summary>
        /// Считает смещение.
        /// </summary>
        /// <param name="data">Массив с данными</param>
        /// <param name="position">Позиция в массиве</param>
        /// <returns>Смещение</returns>
        private uint LongCompressionOffset(byte[] data, ref uint position)
        {
            uint cont = 0;
            byte literalLength = data[position++];
            while (literalLength == 0x00) {
                cont += 0xFF;
                literalLength = data[position++];
            }
            cont += literalLength;
            return cont;
        }

        /// <summary>
        /// Считает двух байтовое смещение.
        /// </summary>
        /// <param name="data">Массив с данными</param>
        /// <param name="position">Позиция в массиве</param>
        /// <param name="literalCount">Количество литералов</param>
        /// <returns>Количество</returns>
        private uint TwoByteOffset(byte[] data, ref uint position, ref uint literalCount)
        {
            byte fb      = data[position++];
            uint cont    = (uint)((fb >> 2) | (data[position++] << 6));
            literalCount = (uint)(fb & 0x03);
            return cont;
        }
    }
}
