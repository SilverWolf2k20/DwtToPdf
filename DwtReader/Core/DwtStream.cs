//-----------------------------------------------------------------------------
// <copyright file="DwtStream.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для считывания данных из файлового или буферного потока.
// </summary>
//-----------------------------------------------------------------------------

using System.Text;

using DwtReader.Components;

namespace DwtReader.Core
{
    /// <summary>
    /// Класс потока данных декодируемого файла.
    /// </summary>
    public class DwtStream : ICloneable
    {
        private Stream _stream;
        private byte _bitPosition;
        private byte _currentByte;

        /// <summary>
        /// Создает буфер на основе файлового потока.
        /// </summary>
        /// <param name="stream">Файловые поток</param>
        public DwtStream(Stream stream)
            => _stream = stream;

        /// <summary>
        /// Создает буфер на основе байтового массива.
        /// </summary>
        /// <param name="buffer">Буффер</param>
        public DwtStream(byte[] buffer)
            => _stream = new MemoryStream(buffer);

        /// <summary>
        /// Создает буфер на основе байтового массива.
        /// </summary>
        /// <param name="buffer">Буффер</param>
        private DwtStream(byte[] buffer, byte currentByte, long position, byte bitPosition)
        {
            _stream = new MemoryStream(buffer);
            _bitPosition = bitPosition;
            _stream.Position = position;
            _currentByte = currentByte;
        }

        /// <summary>
        /// Закрывает поток.
        /// </summary>
        public void Close()
            => _stream.Close();

        /// <summary>
        /// Возвращает размер потока.
        /// </summary>
        /// <returns>Размер потока</returns>
        public long Size()
            => _stream.Length;

        /// <summary>
        /// Устанавливает позицию в потоке.
        /// </summary>
        /// <param name="position">Позиция</param>
        public void SetPosition(long position)
        {
            _bitPosition = 0;
            _stream.Position = position;
        }

        /// <summary>
        /// Возвращает текущую позицию в потоке.
        /// </summary>
        /// <returns>Позиция</returns>
        public long GetPosition()
        {
            if (_bitPosition != 0)
                return _stream.Position - 1;
            return _stream.Position;
        }

        /// <summary>
        /// Устанавливет позицию в байте.
        /// </summary>
        /// <param name="position">Позиция</param>
        public void SetBitPosition(byte position)
        {
            if (position > 7)
                return;

            if (position != 0 && _bitPosition == 0) {
                var buffer = Read();
                _currentByte = buffer;
            }

            if (position == 0 && _bitPosition != 0)
                SetPosition(GetPosition() - 1);

            _bitPosition = position;
        }

        /// <summary>
        /// Возвращает текущую позицию в байте.
        /// </summary>
        /// <returns>Позиция</returns>
        public byte GetBitPosition()
            => _bitPosition;

        /// <summary>
        /// Перемещает указатель на указанное количество бит.
        /// </summary>
        /// <param name="position">Количество бит для перемещения</param>
        public void MoveBitPosition(long position)
        {
            long bits = position + _bitPosition;
            SetPosition(GetPosition() + (bits >> 3));
            _bitPosition = (byte)(bits & 7);

            if (_bitPosition != 0)
                _currentByte = Read();
        }

        /// <summary>
        /// Возвращает один бит (B)
        /// </summary>
        /// <returns>Байт со значением 0/1</returns>
        public byte GetBit()
        {
            if (_bitPosition == 0)
                _currentByte = Read();

            byte ret = (byte)(_currentByte >> (7 - _bitPosition) & 1);
            ++_bitPosition;

            if (_bitPosition == 8)
                _bitPosition = 0;

            return ret;
        }

        /// <summary>
        /// Возвращает два бита (BB)
        /// </summary>
        /// <returns>Два бита</returns>
        public byte Get2Bits()
        {
            byte ret;

            if (_bitPosition == 0)
                _currentByte = Read();

            _bitPosition += 2;

            if (_bitPosition < 9) {
                ret = (byte)(_currentByte >> (8 - _bitPosition));
            }
            else {
                ret = (byte)(_currentByte << 1);
                _currentByte = Read();
                _bitPosition = 1;
                ret = (byte)(ret | _currentByte >> 7);
            }

            if (_bitPosition == 8)
                _bitPosition = 0;

            return (byte)(ret & 3);
        }

        /// <summary>
        /// Возвращает три бита (3B)
        /// </summary>
        /// <returns>Три бита</returns>
        public byte Get3Bits()
        {
            byte ret;

            if (_bitPosition == 0)
                _currentByte = Read();

            _bitPosition += 3;

            if (_bitPosition < 9) {
                ret = (byte)(_currentByte >> (8 - _bitPosition));
            }
            else {
                ret = (byte)(_currentByte << 1);
                _currentByte = Read();
                _bitPosition = 1;
                ret = (byte)(ret | _currentByte >> 7);
            }

            if (_bitPosition == 8)
                _bitPosition = 0;

            return (byte)(ret & 7);
        }

        /// <summary>
        /// Возвращает битовый short (BS)
        /// </summary>
        /// <returns>Битовый short</returns>
        public ushort GetBitShort()
        {
            var b = Get2Bits();
            switch (b) {
                case 0:
                    return GetRawShort();
                case 1:
                    return GetRawChar();
                case 2:
                    return 0;
            }
            return 256;
        }

        /// <summary>
        ///  Возвращает битовый long (BL)
        /// </summary>
        /// <returns>Битовый long</returns>
        public uint GetBitLong()
        {
            var b = Get2Bits();
            switch (b) {
                case 0:
                    return GetRawLong();
                case 1:
                    return GetRawChar();
            }
            return 0;
        }

        /// <summary>
        ///  Возвращает битовый long long (BLL)
        /// </summary>
        /// <returns>Битовый long long</returns>
        public ulong GetBitLongLong()
        {
            var bits = Get3Bits();
            ulong ret = 0;

            for (var i = 0; i < bits; ++i) {
                ret <<= 8;
                ret |= GetRawChar();
            }
            return ret;
        }

        /// <summary>
        ///  Возвращает битовый double (BD)
        /// </summary>
        /// <returns>Битовый double</returns>
        public double GetBitDouble()
        {
            var b = Get2Bits();
            if (b == 1) {
                return 1.0;
            }
            else if (b == 0) {
                byte[] buffer = new byte[8];
                if (_bitPosition != 0)
                    for (var i = 0; i < 8; ++i)
                        buffer[i] = GetRawChar();
                else
                    buffer = Read(8);
                return BitConverter.ToDouble(buffer, 0);
            }
            return 0.0;
        }

        /// <summary>
        ///  Возвращает битовый 3 битовых double (3BD)
        /// </summary>
        /// <returns>3 битовых double</returns>
        public (double x, double y, double z) Get3BitDouble()
        {
            var x = GetBitDouble();
            var y = GetBitDouble();
            var z = GetBitDouble();
            return (x, y, z);
        }

        /// <summary>
        /// Возвращает не сжатый char (RC)
        /// </summary>
        /// <returns>Байт</returns>
        public byte GetRawChar()
        {
            byte ret;
            byte buffer = Read();
            if (_bitPosition == 0) {
                return buffer;
            }
            else {
                ret = (byte)(_currentByte << _bitPosition);
                _currentByte = buffer;
                ret = (byte)(ret | (_currentByte >> (8 - _bitPosition)));
            }
            return ret;
        }

        /// <summary>
        /// Возвращает не сжатый short (RS)
        /// </summary>
        /// <returns>16 бит</returns>
        public ushort GetRawShort()
        {
            byte[] buffer = Read(2);
            ushort ret;

            if (_bitPosition == 0) {
                ret = (ushort)((buffer[1] << 8) | (buffer[0] & 0x00FF));
            }
            else {
                ret = (ushort)((buffer[0] << 8) | (buffer[1] & 0x00FF));
                ret = (ushort)(ret >> (8 - _bitPosition));
                ret = (ushort)(ret | (_currentByte << (8 + _bitPosition)));
                _currentByte = buffer[1];
                ret = (ushort)((ret << 8) | (ret >> 8));
            }
            return ret;
        }

        /// <summary>
        /// Возвращает не сжатый short (RS)
        /// </summary>
        /// <returns>16 бит</returns>
        public ushort GetBeRawShort()
        {
            byte first = GetRawChar();
            byte last  = GetRawChar();
            return (ushort)((first << 8) | (last & 0xFF));
        }

        /// <summary>
        /// Возвращает не сжатый double (RD)
        /// </summary>
        /// <returns>Вещественное число</returns>
        public double GetRawDouble()
        {
            byte[] buffer = new byte[8];
            if (_bitPosition == 0)
                buffer = Read(8);
            else
                for (int i = 0; i < 8; ++i)
                    buffer[i] = GetRawChar();
            return BitConverter.ToDouble(buffer);
        }

        /// <summary>
        /// Возвращает не сжатый long (RL)
        /// </summary>
        /// <returns>32 бита</returns>
        public uint GetRawLong()
        {
            ushort first = GetRawShort();
            ushort last = GetRawShort();
            return (uint)((last << 16) | (first & 0x0000FFFF));
        }

        /// <summary>
        /// Возвращает не сжатый long long (RLL)
        /// </summary>
        /// <returns>64 бита</returns>
        public ulong GetRawLongLong()
        {
            uint first = GetRawLong();
            ulong last = GetRawLong();
            return ((first << 32) | (last & 0x00000000FFFFFFFF));
        }

        /// <summary>
        /// Возвращает не сжатые координаты x, y (2RD)
        /// </summary>
        /// <returns>Координаты x, y</returns>
        public (double x, double y) Get2RawDouble()
        {
            var x = GetRawDouble();
            var y = GetRawDouble();
            return (x, y);
        }

        /// <summary>
        /// Возвращает положительный модульный символ (MC)
        /// </summary>
        /// <returns>Положительный модульный байт</returns>
        public int GetUModularChar()
        {
            int result = 0;
            List<byte> buffer = new();
            for (int i = 0; i < 4; i++) {
                byte bt = GetRawChar();
                buffer.Add((byte)(bt & 0x7F));
                if ((bt & 0x80) == 0)
                    break;
            }

            int offset = 0;
            for (var i = 0; i < buffer.Count; ++i) {
                result += buffer[i] << offset;
                offset += 7;
            }
            return result;
        }

        /// <summary>
        /// Возвращает модульный символ (MC)
        /// </summary>
        /// <returns>Модульный символ</returns>
        public int GetModularChar()
        {
            bool negative = false;
            int result = 0;

            List<byte> buffer = new();
            for (int i = 0; i < 4; i++) {
                byte bt = GetRawChar();
                buffer.Add((byte)(bt & 0x7F));
                if ((bt & 0x80) == 0)
                    break;
            }

            byte b = buffer[buffer.Count - 1];
            if ((b & 0x40) > 0) {
                negative = true;
                buffer.RemoveAt(buffer.Count - 1);
                buffer.Add((byte)(b & 0x3F));
            }

            int offset = 0;
            for (var i = 0; i < buffer.Count; ++i) {
                result += buffer[i] << offset;
                offset += 7;
            }

            if (negative)
                result = -result;
            return result;
        }

        /// <summary>
        /// Возвращает модульный short (MS)
        /// </summary>
        /// <returns>Модульный short</returns>
        public int GetModularShort()
        {
            int result = 0;

            List<int> buffer = new();
            for (int i = 0; i < 2; ++i) {
                int b = GetRawShort();
                buffer.Add(b & 0x7FFF);
                if ((b & 0x8000) == 0)
                    break;
            }

            int offset = 0;
            for (var i = 0; i < buffer.Count; ++i) {
                result += buffer[i] << offset;
                offset += 15;
            }
            return result;
        }

        /// <summary>
        /// Возвращает дескриптор (H)
        /// </summary>
        /// <returns>Дескриптор</returns>
        public (byte code, byte size, uint reference) GetHandle()
        {
            byte data = GetRawChar();

            var code = (byte)((data >> 4) & 0x0F);
            var size = (byte)(data & 0x0F);
            var reference = 0u;
            for (var i = 0; i < size; ++i)
                reference = (reference << 8) | GetRawChar();
            return (code, size, reference);
        }

        /// <summary>
        /// Возвращает десткриптор на основе ссылки.
        /// </summary>
        /// <param name="reference">Сыылка</param>
        /// <returns>Десткриптор</returns>
        public (byte code, byte size, uint reference) GetOffsetHandle(uint reference)
        {
            var handle = GetHandle();
            if (handle.code > 5) {
                switch (handle.code) {
                    case 0x0C:
                        handle.reference = reference - handle.reference;
                        break;
                    case 0x0A:
                        handle.reference = reference + handle.reference;
                        break;
                    case 0x08:
                        handle.reference = reference - 1;
                        break;
                    case 0x06:
                        handle.reference = reference + 1;
                        break;
                }
                handle.code = 7;
            }
            return handle;
        }

        /// <summary>
        /// Возвращет текст (TV)
        /// </summary>
        /// <returns>Текст</returns>
        public string GetVariableText(DwtVersion version, bool nullTerminal)
        {
            if (version > DwtVersion.AC1018)
                return GetUcsText(nullTerminal);

            ushort size = GetBitShort();
            if (size == 0)
                return "";
            byte[] text = GetBytes(size);
            return new string(Encoding.UTF8.GetChars(text));
        }

        /// <summary>
        /// Возвращет тип объекта (OT)
        /// </summary>
        /// <returns>Тип объекта</returns>
        public ushort GetObjectType(DwtVersion version)
        {
            if (version > DwtVersion.AC1021) {
                byte b = Get2Bits();
                if (b == 0)
                    return GetRawChar();
                else if (b == 1)
                    return (ushort)(GetRawChar() + 0x01F0);
                else
                    return GetRawShort();
            }
            return GetBitShort();
        }

        /// <summary>
        /// Возвращет UCS текст (UT)
        /// </summary>
        /// <returns>UCS текст</returns>
        public string GetUcsText(bool nullTerminal)
        {
            var size = GetBitShort();

            if (size == 0)
                return "";
            return Get16BitString(size, nullTerminal);
        }

        /// <summary>
        /// Возвращает 16 битную строку. 2007
        /// </summary>
        /// <returns>Строка</returns>
        public string Get16BitString(ushort size, bool nullTerminal)
        {
            if (size == 0)
                return "";
            size *= 2;
            var textSize = size;

            if (nullTerminal)
                textSize += 2;

            var buffer = GetBytes(textSize);
            Array.Resize(ref buffer, buffer.Length + 2);

            if (!nullTerminal) {
                buffer[size] = (byte)'\0';
                buffer[size + 1] = (byte)'\0';
            }
            return Encoding.Unicode.GetString(buffer);
        }

        /// <summary>
        /// Возвращает значение экструзии (BE)
        /// </summary>
        /// <returns>Значение экструзии</returns>
        public (double x, double y, double z) GetBitExtrusion(bool newStyle)
        {
            if (newStyle == true && GetBit() == 1)
                return (0.0, 0.0, 0.0);

            var x = GetBitDouble();
            var y = GetBitDouble();
            var z = GetBitDouble();
            return (x, y, z);
        }

        /// <summary>
        /// Возвращает вещественное число по умолчанию (BDD)
        /// </summary>
        /// <returns>Вещественное число</returns>
        public double GetDefaultDouble(double d)
        {
            byte b = Get2Bits();

            if (b == 0) {
                return d;
            }
            else if (b == 1) {
                byte[] buffer = new byte[8];
                if (_bitPosition != 0)
                    for (int i = 0; i < 4; ++i)
                        buffer[i] = GetRawChar();
                else
                    buffer = Read(4);

                byte[] tmpBuffer = BitConverter.GetBytes(d);

                for (int i = 0; i < 4; ++i)
                    tmpBuffer[i] = buffer[i];

                return BitConverter.ToDouble(tmpBuffer);
            }
            else if (b == 2) {
                byte[] buffer = new byte[6];
                byte[] tmpBuffer = BitConverter.GetBytes(d);
                if (_bitPosition != 0)
                    for (int i = 0; i < 6; ++i)
                        buffer[i] = GetRawChar();
                else
                    buffer = Read(6);

                for (int i = 2; i < 6; ++i)
                    tmpBuffer[i - 2] = buffer[i];

                tmpBuffer[4] = buffer[0];
                tmpBuffer[5] = buffer[1];
                return BitConverter.ToDouble(tmpBuffer);
            }
            return GetRawDouble();
        }

        /// <summary>
        /// Возвращает значение толщины (BT)
        /// </summary>
        /// <returns>Толщина</returns>
        public double GetBitThickness(bool newStyle)
        {
            if (newStyle)
                if (GetBit() == 1)
                    return 0;
            return GetBitDouble();
        }

        /// <summary>
        /// Возвращает значение цвета (CMC)
        /// </summary>
        /// <returns>Цвет</returns>
        public uint GetCmColor(DwtVersion version)
        {
            if (version < DwtVersion.AC1018)
                return GetBitShort();

            var idx = GetBitShort();
            var rgb = GetBitLong();
            var cb = GetRawChar();

            if ((cb & 1) > 0)
                GetVariableText(version, false);
            if ((cb & 2) > 0)
                GetVariableText(version, false);

            switch (rgb >> 24) {
                case 0xC0:
                    return 256;
                case 0xC1:
                    return 0;
                case 0xC2:
                    return 256;
                case 0xC3:
                    return rgb & 0xFF;
                default:
                    break;
            }
            return 256;
        }

        /// <summary>
        /// Возвращает значение цвета (CMC)
        /// </summary>
        /// <returns>Цвет</returns>
        public uint GetEnColor(DwtVersion version)
        {
            if (version < DwtVersion.AC1018)
                return GetBitShort();

            var index = GetBitShort();
            var flags = index >> 8;
            index &= 0x1FF;

            uint tmp = 0;
            if ((flags & 0x20) > 0)
                tmp = GetBitLong();
            if ((flags & 0x20) > 0)
                tmp = GetBitLong();
            return index;
        }

        /// <summary>
        /// Возвращает байты из потока.
        /// </summary>
        /// <param name="size">Количество байт</param>
        /// <returns>Масиив байт</returns>
        public byte[] GetBytes(long size)
        {
            byte tmp;
            byte[] buffer = Read(size);

            if (_bitPosition != 0) {
                for (var i = 0; i < size; ++i) {
                    tmp = buffer[i];
                    buffer[i] = (byte)((_currentByte << _bitPosition)
                                    | (tmp >> (8 - _bitPosition)));
                    _currentByte = tmp;
                }
            }
            return buffer;
        }

        /// <summary>
        /// Возвращает символ из потока.
        /// </summary>
        /// <returns>Символ</returns>
        public char GetChar()
            => (char)Read();

        /// <summary>
        /// Устанавливает символ в указанную позицию потока.
        /// </summary>
        /// <param name="position">Позиция в потоке</param>
        /// <param name="ch">Символ</param>
        public void SetChar(long position, char ch)
        {
            long pos = GetPosition();
            SetPosition(position);
            _stream.WriteByte((byte)ch);
            SetPosition(pos);
        }

        /// <summary>
        /// Возвращает символы из потока.
        /// </summary>
        /// <param name="size">Количество счиываемых символов</param>
        /// <returns>Масиив символов</returns>
        public char[] GetChars(long size)
        {
            char[] bytes = new char[size];
            for (var i = 0; i < size; ++i)
                bytes[i] = GetChar();
            return bytes;
        }

        /// <summary>
        /// Считывает байты из потока.
        /// </summary>
        /// <param name="size">Количество считываемых байт</param>
        /// <returns>Массив байт</returns>
        /// <remarks>Этот метод игнорирует битовую позицию.</remarks>
        private byte[] Read(long size)
        {
            byte[] buffer = new byte[size];
            for (var i = 0; i < size; ++i)
                buffer[i] = (byte)_stream.ReadByte();
            return buffer;
        }

        /// <summary>
        /// Считывает байт из потока.
        /// </summary>
        /// <returns>Считанный байт</returns>
        /// <remarks>Этот метод игнорирует битовую позицию.</remarks>
        private byte Read()
            => (byte)_stream.ReadByte();

        /// <summary>
        /// Создает копию объекта.
        /// </summary>
        /// <returns>Копия</returns>
        public object Clone()
        {
            // Сохранение данных
            var position    = _stream.Position;
            var bitPosition = _bitPosition;
            var currenByte  = _currentByte;

            // Настройка данных
            SetPosition(0);
            var buffer = GetBytes(_stream.Length);

            // Востановление данных
            _stream.Position = position;
            _bitPosition = bitPosition;
            _currentByte = currenByte;

            return new DwtStream(buffer, currenByte, 0, 0);
        }
    }
}
