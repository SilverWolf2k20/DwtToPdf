//-----------------------------------------------------------------------------
// <copyright file="DwtDecoderAC18.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс для декодирования DWT файла 2004 года.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components;
using DwtReader.Components.Pages;
using DwtReader.Components.Sections;
using DwtReader.Core;
using DwtReader.Data;
using DwtReader.ObjectDecoders.Core;
using DwtReader.Objects.Componets;

using Usl;

namespace DwtReader.Decoders
{
    /// <summary>
    /// Декодировщик для версии формата DWT файла 2004 года.
    /// </summary>
    public partial class DwtDecoderAC18 : IDwtDecoder
    {
        protected DwtFile              _dwtFile;
        protected Dictionary<int, int> _handles;

        #region Constructor
        public DwtDecoderAC18()
        {
            _dwtFile = new DwtFile();
            _handles = new Dictionary<int, int>();
            _dwtFile.Version = DwtVersion.AC1027;
        }
        #endregion

        #region Public methods
        public virtual DwtFile Decode(DwtStream stream)
        {
            // Чтение метаданных.
            DecodeMetaData(stream);
            // TODO: Добавить возвращаемое значение crc.
            DecodeEncryptedMetaData(stream);

            // Чтение карты разделов.
            DecodeSystemSectionPage(stream);
            DecodeDataSectionPage(stream);

            // Чтение рвзделов.
            DecodeSections(stream);

            return _dwtFile;
        }
        #endregion

        #region Metadata
        /// <summary>
        /// Декодирование метаданных.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        private void DecodeMetaData(DwtStream stream)
        {
            Logger.Info("============== Decode Meta Data =============");
            stream.SetPosition(0xB); // Первые1 бит - версия файла + пустые.

            // Чтение данных.
            _dwtFile.MaintenanceVersion = stream.GetRawChar();
            stream.GetRawChar(); // 0x0C: 0x00, 0x01, 0x03
            _dwtFile.PreviewImagePos = stream.GetRawLong();
            _dwtFile.WriterVersion = stream.GetRawChar();
            _dwtFile.ReleaseVersion = stream.GetRawChar();
            _dwtFile.Codepage = stream.GetRawShort();
            stream.GetRawChar(); // Unknown
            stream.GetRawChar(); // Unknown
            stream.GetRawChar(); // Unknown
            _dwtFile.SecurityFlag = stream.GetRawLong();
            stream.GetRawLong(); // Unknown long
            _dwtFile.SummaryInfoAddress = stream.GetRawLong();
            _dwtFile.VBAProjectAddress = stream.GetRawLong();

            // Вывод данных.
            Logger.Debug("Техническая версия:   0x{0:x2}", _dwtFile.MaintenanceVersion);
            Logger.Debug("Превью:               0x{0:x2}", _dwtFile.PreviewImagePos);
            Logger.Debug("Версия писателя:      0x{0:x2}", _dwtFile.WriterVersion);
            Logger.Debug("Релизная версия:      0x{0:x2}", _dwtFile.ReleaseVersion);
            Logger.Debug("Кодовая страница:     {0}     ", _dwtFile.Codepage);
            Logger.Debug("Флаг безопасности:    0x{0:x2}", _dwtFile.SecurityFlag);
            Logger.Debug("Адрес сводной инф.:   0x{0:x2}", _dwtFile.SummaryInfoAddress);
            Logger.Debug("Адрес проекта VBA:    0x{0:x2}", _dwtFile.VBAProjectAddress);
            Logger.Debug("0x00000080:           0x{0:x2}", stream.GetRawLong());
        }

        /// <summary>
        /// Декодирование зашиврованных метаданных.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        private void DecodeEncryptedMetaData(DwtStream stream)
        {
            Logger.Info("========= Decode Encrypted Meta Data ========");
            stream.SetPosition(0x80);

            // Расшифровка заголовка.
            var encrypter = new DwtEncrypter();
            var decryptedData = new DwtStream(encrypter.Decrupt(stream.GetBytes(0x6C)));

            Logger.Debug($"File ID: {new string(decryptedData.GetChars(12)).TrimEnd('\0')}");

            Logger.Debug("0x00 long:                     0x{0:X2}", decryptedData.GetRawLong());
            Logger.Debug("0x6c long:                     0x{0:X2}", decryptedData.GetRawLong());
            Logger.Debug("0x04 long:                     0x{0:X2}", decryptedData.GetRawLong());

            Logger.Debug("Root tree node gap:            0x{0:X2}", decryptedData.GetRawLong());
            Logger.Debug("Lowermost left tree node gap:  0x{0:X2}", decryptedData.GetRawLong());
            Logger.Debug("Lowermost right tree node gap: 0x{0:X2}", decryptedData.GetRawLong());

            Logger.Debug("Unknown long:                  0x{0:X2}", decryptedData.GetRawLong());

            Logger.Debug("Last section page Id:          0x{0:X2}", decryptedData.GetRawLong());
            Logger.Debug("Last section page end address: 0x{0:X2}", decryptedData.GetRawLongLong());
            Logger.Debug("Second header data address:    0x{0:X2}", decryptedData.GetRawLongLong());
            Logger.Debug("Gap amount:                    0x{0:X2}", decryptedData.GetRawLong());
            Logger.Debug("Section page amount:           0x{0:X2}", decryptedData.GetRawLong());

            Logger.Debug("0x20 long:                     0x{0:X2}", decryptedData.GetRawLong());
            Logger.Debug("0x80 long:                     0x{0:X2}", decryptedData.GetRawLong());
            Logger.Debug("0x40 long:                     0x{0:X2}", decryptedData.GetRawLong());

            _dwtFile.SectionPageMapId      = decryptedData.GetRawLong();
            _dwtFile.SectionPageMapAddress = decryptedData.GetRawLongLong() + 0x100;
            _dwtFile.SectionMapId          = decryptedData.GetRawLong();
            _dwtFile.SectionPageArraySize  = decryptedData.GetRawLong();

            Logger.Debug("Section page map Id:           0x{0:X2}", _dwtFile.SectionPageMapId);
            Logger.Debug("Section page map address:      0x{0:X2}", _dwtFile.SectionPageMapAddress);
            Logger.Debug("Section map id:                0x{0:X2}", _dwtFile.SectionMapId);
            Logger.Debug("Section page array size:       0x{0:X2}", _dwtFile.SectionPageArraySize);

            Logger.Debug("Gap array size:                0x{0:X2}", decryptedData.GetRawLong());
            var readCrc = decryptedData.GetRawLong();

            // Костыльное заполнение
            for (var i = 0x68; i < 0x6c; ++i)
                decryptedData.SetChar(i, '\0');

            var crc = new CyclicRedundancyCheck();
            Logger.Trace($"Checksum is correct?: {crc.Checksum(decryptedData, 0, 0, 0x6C, readCrc)}");
        }
        #endregion

        #region SectionsMap
        /// <summary>
        /// Декодирование страниц системного раздела.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSystemSectionPage(DwtStream stream)
        {
            Logger.Info("========= Decode System Section Page ========");
            stream.SetPosition((long)_dwtFile.SectionPageMapAddress);

            uint type = stream.GetRawLong();
            Logger.Debug("Тип секции:        0x{0:X2}", type);

            if (type != 0x41630E3B) {
                Logger.Error("Неверный тип системной страницы!");
                return false;
            }

            var buffer = new DwtStream(ParseSystemPage(stream));
            var startPosition = 0x100U;

            for (var i = 1U; i < _dwtFile.SectionPageArraySize - 1; ++i) {
                var section     = new SystemSection();
                section.Number = (int)buffer.GetRawLong();
                section.Size = buffer.GetRawLong();
                section.Address = startPosition;

                if (section.Number < 0) {
                    var page     = new SystemPage();
                    page.Parent = buffer.GetRawLong();
                    page.Left = buffer.GetRawLong();
                    page.Right = buffer.GetRawLong();
                    section.Page = page;
                }

                _dwtFile.SystemSections[section.Number] = section;
                startPosition += section.Size;
            }

#if DEBUG
            foreach (var section in _dwtFile.SystemSections.Values) {
                Logger.Trace("Номер:  {0}     ", section.Number);
                Logger.Debug("Размер: 0x{0:X2}", section.Size);
                Logger.Debug("Адрес:  0x{0:X2}", section.Address);

                if (section.Page is not null) {
                    Logger.Debug("Родитель: 0x{0:X2}", section.Page.Parent);
                    Logger.Debug("Лево:     0x{0:X2}", section.Page.Left);
                    Logger.Debug("Право:    0x{0:X2}", section.Page.Right);
                }
            }
#endif
            return true;
        }

        /// <summary>
        /// Декодирование страниц раздела данных.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeDataSectionPage(DwtStream stream)
        {
            Logger.Info("========== Decode Data Section Page =========");
            stream.SetPosition(_dwtFile.SystemSections[(int)_dwtFile.SectionMapId].Address);

            uint type = stream.GetRawLong();
            Logger.Debug("Тип раздела:       0x{0:X2}", type);

            if (type != 0x4163003B) {
                Logger.Error("Неверный тип системной страницы!");
                return false;
            }

            var buffer = new DwtStream(ParseSystemPage(stream));

            _dwtFile.DataSectionsNumber = buffer.GetRawLong();
            Logger.Debug("Sections: {0}", _dwtFile.DataSectionsNumber);
            Logger.Debug("0x02:     0x{0:x2}", buffer.GetRawLong());
            Logger.Debug("0x7400:   0x{0:x2}", buffer.GetRawLong());
            Logger.Debug("0x00:     0x{0:x2}", buffer.GetRawLong());
            Logger.Debug("Unknown:  0x{0:x2}", buffer.GetRawLong());

            for (var i = 0; i < _dwtFile.DataSectionsNumber; ++i) {
                var section         = new DataSection();
                section.Size = buffer.GetRawLongLong();
                section.PageCount = buffer.GetRawLong();
                section.MaxSize = buffer.GetRawLong();
                buffer.GetRawLong(); // Unknown.
                section.IsCompressed = buffer.GetRawLong();
                section.Id = buffer.GetRawLong();
                section.IsEncrypted = buffer.GetRawLong();
                section.Name = new string(buffer.GetChars(64));
                section.Name = section.Name.Substring(0, section.Name.IndexOf('\0'));

                for (var j = 0; j < section.PageCount; ++j) {
                    var page = new DataPage();
                    page.Number = buffer.GetRawLong();
                    page.Size = buffer.GetRawLong();
                    page.Offset = buffer.GetRawLongLong();
                    section.Pages[page.Number] = page;
                }

                _dwtFile.DataSections[section.ToEnum()] = section;
            }

            foreach (var section in _dwtFile.DataSections.Values) {
                Logger.Trace("Name:   {0}     ", section.Name);
                Logger.Debug("Size:   0x{0:x2}", section.Size);
                Logger.Debug("Count:  0x{0:x2}", section.PageCount);
                Logger.Debug("MaxSze: 0x{0:x2}", section.MaxSize);
                Logger.Debug("Comp?:  0x{0:x2}", section.IsCompressed);
                Logger.Debug("Id:     0x{0:x2}", section.Id);
                Logger.Debug("Enc?:   0x{0:x2}", section.IsEncrypted);
                foreach (var page in section.Pages.Values) {
                    Logger.Trace("Name:     0x{0:x2}", page.Number);
                    Logger.Trace("Size:     0x{0:x2}", page.Size);
                    Logger.Trace("Offs:     0x{0:x2}", page.Offset);
                }
            }
            return true;
        }

        /// <summary>
        /// Парсинг системного раздела.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>Распакованные данные</returns>
        protected virtual byte[] ParseSystemPage(DwtStream stream)
        {
            uint decompressionSize = stream.GetRawLong();
            uint compressionSize = stream.GetRawLong();

            Logger.Debug("Размер не сжатого: {0}", decompressionSize);
            Logger.Debug("Размер сжатого:    {0}", compressionSize);
            Logger.Debug("Тип сжатия:        {0}", stream.GetRawLong());
            Logger.Debug("CRC:               0x{0:X2}", stream.GetRawLong());

            stream.MoveBitPosition(-160);
            var data = stream.GetBytes(20);

            for (var i = 16; i < 20; ++i)
                data[i] = 0;

            uint controlSum = SectionCheckSum(0, data);
            Logger.Debug("CRC заголовка:     0x{0:X2}", controlSum);

            var compressedData = stream.GetBytes(compressionSize);
            controlSum = SectionCheckSum((int)controlSum, compressedData);
            Logger.Debug("CRC даных:         0x{0:X2}", controlSum);

            Logger.Info($"Распаковка {compressionSize} байт из {decompressionSize} байт.");
            DwtCompressor compressor = new DwtCompressor();

            return compressor.Decompress(compressedData, decompressionSize);
        }

        /// <summary>
        /// Парсинг раздела с данными.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <param name="dataSection">Данные раздела с данными</param>
        /// <returns>Распакованные данные</returns>
        protected virtual byte[] ParseDataPage(DwtStream stream, DataSection dataSection)
        {
            byte[] dataOut = new byte[dataSection.Size];

            foreach (var page in dataSection.Pages.Values) {
                var system = _dwtFile.SystemSections[(int)page.Number];

                Logger.Trace("Id:               0x{0:x2}", page.Number);
                Logger.Debug("Размер:           0x{0:x2}", system.Size);
                Logger.Debug("Адрес:            0x{0:x2}", system.Address);
                Logger.Debug("Сдвиг:            0x{0:x2}", page.Offset);

                stream.SetPosition(system.Address);
                var compressor   = new DwtCompressor();
                var headerBuffer = stream.GetBytes(32);
                headerBuffer = compressor.Decrypt18(headerBuffer, system.Address);

                var buffer = new DwtStream(headerBuffer);
                Logger.Debug("Тип:               0x{0:X2}", buffer.GetRawLong());
                Logger.Debug("Номер:             0x{0:X2}", buffer.GetRawLong());
                var compressSize   = buffer.GetRawLong();
                var decompressSize = buffer.GetRawLong();
                Logger.Debug("Размер не сжатого: 0x{0:X2}", decompressSize);
                Logger.Debug("Размер сжатого:    0x{0:X2}", compressSize);
                Logger.Debug("Сдвиг:             0x{0:X2}", buffer.GetRawLong());
                Logger.Debug("Unknown:           0x{0:X2}", buffer.GetRawLong());
                Logger.Debug("CRC заголовка:     0x{0:X2}", buffer.GetRawLong());
                Logger.Debug("CRC данных:        0x{0:X2}", buffer.GetRawLong());

                stream.SetPosition(system.Address + 32);
                var compressData = stream.GetBytes(compressSize);

                var sumData = SectionCheckSum(0, compressData);
                for (var i = 24; i < 28; ++i)
                    headerBuffer[i] = 0;
                var sumHead = SectionCheckSum((int)sumData, headerBuffer);
                Logger.Debug("CRC заголовка:     0x{0:X2}", sumHead);
                Logger.Debug("CRC даных:         0x{0:X2}", sumData);

                // Распаковка данных
                byte[] data = compressor.Decompress(compressData, (uint)dataSection.MaxSize);

                for (var i = page.Offset; i < dataSection.Size && i - page.Offset < (ulong)data.Length; ++i)
                    dataOut[i] = data[i - page.Offset];
            }
            return dataOut;
        }

        /// <summary>
        /// Подсчет контрольной суммы раздела.
        /// </summary>
        /// <param name="seed">Сид</param>
        /// <param name="data">Массив данных</param>
        /// <returns>Контрольная сумма</returns>
        protected virtual uint SectionCheckSum(int seed, byte[] data)
        {
            int size = data.Length;
            int sum1 = seed & 0xffff;
            int sum2 = seed >> 0x10;

            while (size != 0) {
                int chunkSize = 0x15b0 < size ? 0x15b0 : size;
                size -= chunkSize;
                for (var i = 0; i < chunkSize; i++) {
                    sum1 += data[i];
                    sum2 += sum1;
                }
                sum1 %= 0xFFF1;
                sum2 %= 0xFFF1;
            }
            return (uint)((sum2 << 0x10) | (sum1 & 0xffff));
        }
        #endregion

        /// <summary>
        /// Декодирование разделов.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        protected virtual void DecodeSections(DwtStream stream)
        {
            // Основные разделы
            DecodeSectionHeader(stream);
            DecodeSectionHandles(stream);
            DecodeSectionClass(stream);
            DecodeSectionObjects(stream);

            // Дополнительные разделы
            DecodeSectionQoutes(stream);
            DecodeSectionSummaryInfo(stream);
            DecodeSectionPreview(stream);
            DecodeSectionVBAProject(stream);
            DecodeSectionAppInfo(stream);
            DecodeSectionFileDependenciesList(stream);
            DecodeSectionRevHistory(stream);
            DecodeSectionSecurity(stream);
            DecodeSectionObjectsFreeSpace(stream);
            DecodeSectionTemplate(stream);
            DecodeSectionPrototype(stream);
            DecodeSectionUnknown(stream);
            DecodeSectionSecondHeader(stream);
            DecodeSectionAuxHeader(stream);
        }

        #region Section decoders
        /// <summary>
        /// Декодирование раздела заголовка.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionHeader(DwtStream stream)
        {
            Logger.Info("=========== Decode Section Header ===========");
            DataSection dataSection;

            try {
                dataSection = _dwtFile.DataSections[TypeSection.Header];
            }
            catch (Exception) {
                Logger.Error("Отсутствует секция заголовка.");
                return false;
            }

            var buffer = new DwtStream(ParseDataPage(stream, dataSection));

            for (var i = 0; i < 16; ++i)
                Console.Write("0x{0:X2} ", buffer.GetRawChar());
            Console.WriteLine();

            // Расшифровка данных заголовка
            return true;
        }

        /// <summary>
        /// Декодирование раздела "".
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionQoutes(DwtStream stream)
        {
            if (_dwtFile.Version < DwtVersion.AC1032)
                return true;
            Logger.Info("Decode Section Qoutes");
            return true;
        }

        /// <summary>
        /// Декодирование раздела сводной информации.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionSummaryInfo(DwtStream stream)
        {
            Logger.Info("Decode Section Summary Info");
            return true;
        }

        /// <summary>
        /// Декодирование раздела с изображением для предпоказа.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionPreview(DwtStream stream)
        {
            Logger.Info("Decode Section Preview");
            return true;
        }

        /// <summary>
        /// Декодирование раздела VBA проекта.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionVBAProject(DwtStream stream)
        {
            Logger.Info("Decode Section VBA Project");
            return true;
        }

        /// <summary>
        /// Декодирование раздела информации о приложении.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionAppInfo(DwtStream stream)
        {
            Logger.Info("Decode Section App Info");
            return true;
        }

        /// <summary>
        /// Декодирование раздела зависимых файлов.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionFileDependenciesList(DwtStream stream)
        {
            Logger.Info("Decode Section File Dependencies List");
            return true;
        }

        /// <summary>
        /// Декодирование раздела повторной истории.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionRevHistory(DwtStream stream)
        {
            Logger.Info("Decode Section RevHistory");
            return true;
        }

        /// <summary>
        /// Декодирование раздела безопасности.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionSecurity(DwtStream stream)
        {
            Logger.Info("Decode Section Security");
            return true;
        }

        /// <summary>
        /// Декодирование раздела объектов.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionObjects(DwtStream stream)
        {
            Logger.Info("=========== Decode Section Objects ========== ");
            DataSection dataSection;

            try {
                dataSection = _dwtFile.DataSections[TypeSection.Objects];
            }
            catch (Exception) {
                Logger.Error("Отсутствует секция объектов.");
                return false;
            }

            var buffer  = new DwtStream(ParseDataPage(stream, dataSection));

            Logger.Info($"Всего {_handles.Values.Count} объектов.");
            foreach (var location in _handles.Values) {
                buffer.SetPosition(location);

                // Common:
                int size = buffer.GetModularShort(); // Размер объекта.
                // R2010+:
                int bitSize = buffer.GetUModularChar(); // Размер потока дескрипторов.

                var entityBuffer = new DwtStream(buffer.GetBytes(size));
                int type = entityBuffer.GetObjectType(_dwtFile.Version);

                Logger.Trace($"Тип:            0x{type:X3}");

                if (type > 499) { // Объекты с нефиксированным значением.
                    Logger.Warn($"Найден объект управления.");
                    continue;
                }

                var factory = new EntityDecoderFactory();
                var entityDecoders = factory.Create((ObjectType)type);
                var entity = entityDecoders?.Decode(entityBuffer, bitSize, null);

                if (entity is not null)
                    _dwtFile.Objects.Add((entity.Type, entity));
            }
            return true;
        }

        /// <summary>
        /// Декодирование раздела свободного объектного пространства.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns><True - если чтение прошло успешно, иначе - false/returns>
        protected virtual bool DecodeSectionObjectsFreeSpace(DwtStream stream)
        {
            Logger.Info("Decode Section Objects Free Space");
            return true;
        }

        /// <summary>
        /// Декодирование раздела шаблонов.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionTemplate(DwtStream stream)
        {
            Logger.Info("Decode Section Template");
            return true;
        }

        /// <summary>
        /// Декодирование раздела дескрипторов.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionHandles(DwtStream stream)
        {
            Logger.Info("=========== Decode Section Handles ==========");
            DataSection dataSection;

            try {
                dataSection = _dwtFile.DataSections[TypeSection.Handles];
            }
            catch (Exception) {
                Logger.Error("Отсутствует секция заголовка.");
                return false;
            }

            var buffer = new DwtStream(ParseDataPage(stream, dataSection));

            var address = 0U;
            buffer.SetPosition(address);

            uint maxPosition = address + (uint)dataSection.Size;
            Logger.Debug($" Адрес:         {address}");
            Logger.Debug($" Размер:        {dataSection.Size}");
            Logger.Debug($" Макс. позиция: {maxPosition}");

            long startPosition = address;

            while (maxPosition > buffer.GetPosition()) {
                ushort size = buffer.GetBeRawShort();
                Logger.Debug($"Размер секции: {size}");
                buffer.SetPosition(startPosition);

                var handleBuffer = new DwtStream(buffer.GetBytes(size));
                if (size != 2) {
                    handleBuffer.SetPosition(2);
                    int lastHandle = 0;
                    int lastLoc = 0;

                    while (handleBuffer.GetPosition() < size) {
                        lastHandle += handleBuffer.GetUModularChar();
                        lastLoc += handleBuffer.GetModularChar();
#if DEBUG
                        Logger.Trace("handle: 0x{0:X2}\tloc: {1}", lastHandle, lastLoc);
#endif
                        _handles[lastHandle] = lastLoc;
                    }
                }

                // Расчет контрольной суммы
                var    crc          = new CyclicRedundancyCheck();
                uint   checkSum     = crc.Calculate8Bit(handleBuffer, 0xC0C1, 0, size);
                ushort checkSumRead = buffer.GetBeRawShort();
                Logger.Debug($"Считанная контр. сумма:   {checkSumRead}");
                Logger.Debug($"Посчитанная контр. сумма: {checkSum}");
                startPosition = buffer.GetPosition();
            }
            return true;
        }

        /// <summary>
        /// Декодирование раздела прототипов.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionPrototype(DwtStream stream)
        {
            Logger.Info("Decode Section Prototype");
            return true;
        }

        /// <summary>
        /// Декодирование неизвестного раздела.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionUnknown(DwtStream stream)
        {
            Logger.Info("Decode Section Unknown");
            return true;
        }

        /// <summary>
        /// Декодирование раздела второго заголовка.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionSecondHeader(DwtStream stream)
        {
            Logger.Info("Decode Section Second Header");
            return true;
        }

        /// <summary>
        /// Декодирование раздела вспомогательной информации.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionAuxHeader(DwtStream stream)
        {
            Logger.Info("Decode Section AuxHeader");
            return true;
        }

        /// <summary>
        /// Декодирование раздела классов.
        /// </summary>
        /// <param name="stream">Файловый поток</param>
        /// <returns>True - если чтение прошло успешно, иначе - false</returns>
        protected virtual bool DecodeSectionClass(DwtStream stream)
        {
            Logger.Info("============ Decode Section Class =========== ");
            DataSection dataSection;

            try {
                dataSection = _dwtFile.DataSections[TypeSection.Classes];
            }
            catch (Exception) {
                Logger.Error("Отсутствует секция классов.");
                return false;
            }

            var buffer  = new DwtStream(ParseDataPage(stream, dataSection));

            // 0x8D 0xA1 0xC4 0xB8 0xC4 0xA9 0xF8 0xC5 0xC0 0xDC 0xF4 0x5F 0xE7 0xCF 0xB6 0x8A
            for (var i = 0; i < 16; ++i)
                Console.Write("0x{0:X2} ", buffer.GetRawChar());
            Console.WriteLine();

            Logger.Debug($"Size:           {buffer.GetRawLong()}");

            // R2010+
            if(_dwtFile.MaintenanceVersion > 3)
                Logger.Debug($"Unknown:        {buffer.GetRawLong()}");

            // R2004+
            var bitSize = buffer.GetRawLong();
            Logger.Debug($"BitSize:        {bitSize}");

            var classNumber = buffer.GetBitShort();
            Logger.Debug($"ClassNumber:    {classNumber}");
            Logger.Debug($"0x00:           {buffer.GetRawChar()}");
            Logger.Debug($"0x00:           {buffer.GetRawChar()}");
            Logger.Debug($"True:           {Convert.ToBoolean(buffer.GetBit())}");

            var stringBuffer = (DwtStream)buffer.Clone();
            var startPosition = bitSize + 191;
            Logger.Debug($"Start position: {startPosition}");

            stringBuffer.SetPosition(startPosition >> 3);
            stringBuffer.SetBitPosition((byte)(startPosition & 7));

            Logger.Debug($"EndBit:         {stringBuffer.GetBit()}");
            startPosition -= 16;

            stringBuffer.SetPosition(startPosition >> 3);
            stringBuffer.SetBitPosition((byte)(startPosition & 7));

            var strDataSize = stringBuffer.GetRawShort();
            Logger.Debug($"strDataSize:    {strDataSize}");

            if ((strDataSize & 0x8000) > 0) {
                startPosition -= 16;
                strDataSize &= 0x7FFF;
                stringBuffer.SetPosition(startPosition >> 3);
                stringBuffer.SetBitPosition((byte)(startPosition & 7));
                strDataSize |= (ushort)(stringBuffer.GetRawShort() << 15);
            }
            startPosition -= strDataSize;

            stringBuffer.SetPosition(startPosition >> 3);
            stringBuffer.SetBitPosition((byte)(startPosition & 7));

            var endDataPos = classNumber - 499;
            for (int i = 0; i < endDataPos; ++i) {
                Logger.Debug($"Class number:   {buffer.GetBitShort()}");
                Logger.Debug($"Proxy flag:     {buffer.GetBitShort()}");

                Logger.Debug($"App name:       {stringBuffer.GetVariableText(DwtVersion.AC1027, false).TrimEnd('\0')}");
                Logger.Debug($"Class name:     {stringBuffer.GetVariableText(DwtVersion.AC1027, false).TrimEnd('\0')}");
                Logger.Trace($"Dxf rec name:   {stringBuffer.GetVariableText(DwtVersion.AC1027, false).TrimEnd('\0')}");

                Logger.Debug($"Baba Kapa flag: {buffer.GetBit()}"); // Proxy capabilities flag
                Logger.Debug($"Entity flag:    {buffer.GetBitShort()}");

                // 2004+:
                Logger.Debug($"Instance count: {buffer.GetBitLong()}");
                Logger.Debug($"DWG version:    {buffer.GetBitLong()}");
                Logger.Debug($"Maintenance:    {buffer.GetBitLong()}");
                Logger.Debug($"Unknown:        {buffer.GetBitLong()}");
                Logger.Debug($"Unknown:        {buffer.GetBitLong()}");
            }

            stringBuffer.SetPosition(stringBuffer.GetPosition() + 1);
            Logger.Debug($"CRC:            {stringBuffer.GetRawShort()}");
            Logger.Debug($"CRC unknown:    {stringBuffer.GetRawShort()}");

            for (var i = 0; i < 16; ++i)
                Console.Write("0x{0:X2} ", stringBuffer.GetRawChar());
            Console.WriteLine();

            return true;
        }
        #endregion
    }
}
