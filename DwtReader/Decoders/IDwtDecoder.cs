//-----------------------------------------------------------------------------
// <copyright file="DwtDecoderAC18.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит интерфейс декодировщика файла.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Core;
using DwtReader.Data;

namespace DwtReader.Decoders
{
    /// <summary>
    /// Интерфейс декодировщика файла.
    /// </summary>
    public interface IDwtDecoder
    {
        /// <summary>
        /// Декодировать файл.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>Данные файла</returns>
        public DwtFile Decode(DwtStream stream);
    }
}
