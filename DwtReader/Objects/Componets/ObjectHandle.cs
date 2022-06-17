//-----------------------------------------------------------------------------
// <copyright file="ObjectHandle.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс данных дескриптора сущности.
// </summary>
//-----------------------------------------------------------------------------

namespace DwtReader.Objects.Componets
{
    /// <summary>
    /// Данные дескриптора сущности.
    /// </summary>
    public class ObjectHandle
    {
        public byte code = default;
        public byte size = default;
        public uint reference = default;

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="handle">Дескриптор (код, размер, ссылка)</param>
        public ObjectHandle(Tuple<byte, byte, uint> handle)
        {
            code      = handle.Item1;
            size      = handle.Item2;
            reference = handle.Item3;
        }
    }
}
