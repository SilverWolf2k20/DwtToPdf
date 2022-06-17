//-----------------------------------------------------------------------------
// <copyright file="DwtDecoderFactory.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс фабрики декодировщиков.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components;

namespace DwtReader.Decoders
{
    /// <summary>
    /// Фабрика декодировщиков.
    /// </summary>
    public class DwtDecoderFactory
    {
        private Dictionary<DwtVersion, IDwtDecoder> _decoder = new();

        /// <summary>
        /// Конструктор фабрики, добавляющий декодировщики.
        /// </summary>
        public DwtDecoderFactory()
        {
            _decoder.Add(DwtVersion.AC1027, new DwtDecoderAC27());
        }

        /// <summary>
        /// Создает декодировщик исходя из версии DWT файла.
        /// </summary>
        /// <param name="version">Версия DWT файла</param>
        /// <returns>Декодировщик</returns>
        public IDwtDecoder Create(DwtVersion version)
        {
            if (!_decoder.ContainsKey(version))
                return null;
            return _decoder[version];
        }

        /// <summary>
        /// Добавляет декодировщик в фабрику.
        /// </summary>
        /// <param name="version">Версия DWT файла</param>
        /// <param name="decoder">Декодировщик</param>
        public void Add(DwtVersion version, IDwtDecoder decoder)
            => _decoder.Add(version, decoder);
    }
}
