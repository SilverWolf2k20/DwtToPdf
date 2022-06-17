//-----------------------------------------------------------------------------
// <copyright file="DwtFileReader.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для чтения файла в формате DWT.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components;
using DwtReader.Core;
using DwtReader.Data;
using DwtReader.Decoders;

namespace DwtReader
{
    /// <summary>
    /// Состояние модуля чтения файла.
    /// </summary>
    public enum ReaderState
    {
        Success,
        FileIsBig,
        VersionNotSupported,
        FatalError,
        FileIsDwg,
        InvalidFileName,
        PathIsNull,
    }

    /// <summary>
    /// Класс для чтения файла.
    /// </summary>
    public class DwtFileReader
    {
        /// <summary>
        /// Путь к файлу.
        /// </summary>
        private readonly string _filePath = string.Empty;

        /// <summary>
        /// Версия файла.
        /// </summary>
        private DwtVersion _version = DwtVersion.None;

        /// <summary>
        /// Состояние модуля чтения.
        /// </summary>
        public ReaderState State { get; private set; } = ReaderState.Success;

        /// <summary>
        /// Создает экземпляр класса DwtFileReader.
        /// </summary>
        /// <param name="filePath"></param>
        public DwtFileReader(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Анализ открываемого файла.
        /// </summary>
        /// <returns></returns>
        public bool FileAnalysis()
        {
            // Определить корректность формата файла.
            if (IsValidDwtFileName() == false)
                return false; 
            
            // Обработка файла.
            try {
                var stream = new DwtStream(File.Open(_filePath, FileMode.Open));
                // Определение размера файла.
                if (stream.Size() > 1024 * 1024 * 50)
                    State = ReaderState.FileIsBig;
            
                stream.Close();
            }
            catch (Exception) {
                State = ReaderState.FatalError;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Прочитать DWT файл.
        /// </summary>
        /// <returns>Объект с данными файла</returns>
        public DwtFile? Read()
        {
            // Создать объект файла.
            DwtFile? dwtFile = null;

            try {
                // Создать файловый поток и определить версию формата.
                var stream  = new DwtStream(File.Open(_filePath, FileMode.Open));
                _version    = DetermineVersion(stream);
                // Создать фабрику и получить от нее декодер.
                var factory = new DwtDecoderFactory();
                var decoder = factory.Create(_version);

                // Проверить существование декодера.
                if (decoder == null) {
                    State = ReaderState.VersionNotSupported;
                    stream.Close();
                }

                // Прочитать файл.
                dwtFile = decoder?.Decode(stream);
            }
            catch (Exception) {
                State = ReaderState.FatalError;
            }
            return dwtFile;
        }

        /// <summary>
        /// Определяет версию DWT файла.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>Версия файла. Если None, то версия неизвестна</returns>
        private DwtVersion DetermineVersion(DwtStream stream)
        {
            stream.SetPosition(0);
            var version = new string(stream.GetChars(6));
            try {
                return (DwtVersion)Enum.Parse(typeof(DwtVersion), version);
            }
            catch (Exception) {
                return DwtVersion.None;
            }
        }

        /// <summary>
        /// Определяет корректность выбранного файла.
        /// </summary>
        /// <returns>True - елси файл корректен, иначе false</returns>
        private bool IsValidDwtFileName()
        {
            if (_filePath is null) {
                State = ReaderState.PathIsNull;
                return false;
            }

            if (Path.GetExtension(_filePath).ToLower() == ".dwg") {
                State = ReaderState.FileIsDwg;
                return true;
            }

            if (Path.GetExtension(_filePath).ToLower() == ".dwt")
                return true;

            State = ReaderState.InvalidFileName;
            return false;
        }
    }
}
