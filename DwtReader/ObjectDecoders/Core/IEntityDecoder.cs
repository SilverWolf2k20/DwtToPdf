//-----------------------------------------------------------------------------
// <copyright file="IEntityDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит интерфейс декодировщика сущности.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Core;
using DwtReader.Objects;

namespace DwtReader.ObjectDecoders.Core
{
    /// <summary>
    /// Интерфейс декодировщика сущности.
    /// </summary>
    internal interface IEntityDecoder
    {
        /// <summary>
        /// Считывает информацию о сущности.
        /// </summary>
        /// <param name="stream">Поток с данными</param>
        /// <param name="bitSize">Размер бита</param>
        /// <param name="entity">Сущнось для записи данных</param>
        /// <returns>Сущность с данными</returns>
        public Entity Decode(DwtStream stream, int bitSize, Entity entity);
    }
}
