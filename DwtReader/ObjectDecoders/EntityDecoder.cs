//-----------------------------------------------------------------------------
// <copyright file="EntityDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика сущности.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components;
using DwtReader.Core;
using DwtReader.ObjectDecoders.Core;
using DwtReader.Objects;
using DwtReader.Objects.Componets;

using Usl;

namespace DwtReader.ObjectDecoders
{
    /// <summary>
    /// Декодировщик сущности.
    /// </summary>
    internal class EntityDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            // R2000+:
            entity.Size = stream.Size() * 8 - bitSize;
            Logger.Debug($"Размер:         {entity.Size}");
            // Common:
            entity.Handle = new ObjectHandle(stream.GetHandle().ToTuple());

            // Чтение расширенных данных.
            var sizeExtended = stream.GetBitShort();
            while (sizeExtended > 0)
                sizeExtended = DecodeExtendedObjectData(stream, sizeExtended);

            // Чтение данных графического изображения.
            if (stream.GetBit() > 1)
                DecodeGraphicImage(stream);

            if (stream.Get2Bits() == 0)
                entity.HaveLink = true;

            entity.NumberReactor = stream.GetBitShort();

            // R2000+:
            entity.XDictFlag = Convert.ToBoolean(stream.GetBit());
            Logger.Debug($"xDictFlag:      {entity.XDictFlag}");
            Logger.Debug($"Have Links:     {stream.GetBit()}");

            entity.Color = stream.GetEnColor(DwtVersion.AC1027);
            entity.LineTypeScale = stream.GetBitDouble();

            entity.LineType = (LineAndPlotType)stream.Get2Bits();
            entity.PlotStyle = (LineAndPlotType)stream.Get2Bits();
            // R2007+:
            entity.Material = (MaterialType)stream.Get2Bits();
            entity.Shadow = stream.GetRawChar();
            // R2010+:
            entity.HasFullVisualStyle = Convert.ToBoolean(stream.GetBit());
            entity.HasFaceVisualStyle = Convert.ToBoolean(stream.GetBit());
            entity.HasEdgeVisualStyle = Convert.ToBoolean(stream.GetBit());
            // Common:
            entity.Invisible = stream.GetBitShort();
            // R2000+:
            entity.LineWeight = stream.GetRawChar();

            Logger.Debug($"Color:          {entity.Color}");
            Logger.Debug($"Linetype scale: {entity.LineTypeScale}");
            Logger.Debug($"Line type flag: {entity.LineType}");
            Logger.Debug($"Plotstyle flag: {entity.PlotStyle}");

            Logger.Debug($"Material flag:  {entity.Material}");
            Logger.Debug($"Shadow flag:    {entity.Shadow}");
            Logger.Debug($"Invisible:      {entity.Invisible}");
            Logger.Debug($"Lineweight:     {entity.LineWeight}");

            Logger.Debug($"HaveLink:       {entity.HaveLink}");
            Logger.Debug($"NumReactors:    {entity.NumberReactor}");

            Logger.Debug($"Full visual:    {entity.HasFullVisualStyle}");
            Logger.Debug($"Face visual:    {entity.HasFaceVisualStyle}");
            Logger.Debug($"Edge visual:    {entity.HasEdgeVisualStyle}");

            Logger.Debug($"Handle:         {entity.Handle.code}, {entity.Handle.size}, 0x{entity.Handle.reference:X}");

            return entity;
        }

        /// <summary>
        /// Считывает информацию о родителях.
        /// </summary>
        /// <param name="stream">Поток с данными</param>
        /// <param name="entity">Сущность для записи данных</param>
        /// <returns>Сущность с данными</returns>
        public Entity DecodeOwner(DwtStream stream, Entity entity)
        {
            stream.SetPosition(entity.Size >> 3);
            stream.SetBitPosition((byte)(entity.Size & 7));

            // Common:
            if (entity.HaveLink) {
                entity.Owner = new ObjectHandle(stream.GetOffsetHandle(entity.Handle.reference).ToTuple());
                Logger.Debug($"Блок:           {entity.Owner.code}, {entity.Owner.size}, 0x{entity.Owner.reference:X}");
            }

            for (int i = 0; i < entity.NumberReactor; ++i)
                Logger.Debug($"Реактор {i}:    {stream.GetHandle().ToTuple()}");

            if (entity.XDictFlag == true)
                Logger.Debug($"XDicObject:     {stream.GetHandle().ToTuple()}");

            // R2004+:
            //Logger.Debug($"Цвета:          {stream.GetHandle().ToTuple()}");
            // R2000+:
            // TODO: Исправить вывод дескриптора слоя.
            Logger.Debug($"Layer:          {stream.GetOffsetHandle(entity.Handle.reference).ToTuple()}");

            if (entity.LineType == LineAndPlotType.InHandlesSection)
                Logger.Debug($"LineType:       {stream.GetOffsetHandle(entity.Handle.reference).ToTuple()}");
            // R2007:
            if (entity.Material == MaterialType.InHandlesSection)
                Logger.Debug($"Material:       {stream.GetOffsetHandle(entity.Handle.reference).ToTuple()}");
            // R2000:
            if (entity.PlotStyle == LineAndPlotType.InHandlesSection)
                Logger.Debug($"PlotStyle:      {stream.GetOffsetHandle(entity.Handle.reference).ToTuple()}");
            // R2010+:
            if (entity.HasFullVisualStyle == true)
                Logger.Debug($"FullVisual:     {stream.GetOffsetHandle(entity.Handle.reference).ToTuple()}");
            if (entity.HasFaceVisualStyle == true)
                Logger.Debug($"FaceVisual:     {stream.GetOffsetHandle(entity.Handle.reference).ToTuple()}");
            if (entity.HasEdgeVisualStyle == true)
                Logger.Debug($"EdgeVisual:     {stream.GetOffsetHandle(entity.Handle.reference).ToTuple()}");

            return entity;
        }

        /// <summary>
        /// Настраивает строку на ввывод текста.
        /// </summary>
        /// <param name="stream">Поток с данными</param>
        /// <param name="size">Размер чанка данных</param>
        public void NormalizationStringStream(DwtStream stream, long size)
        {
            stream.MoveBitPosition(size - 1);
            if (stream.GetBit() == 1) {
                stream.MoveBitPosition(-17);
                var dataSize = stream.GetRawShort();
                if ((dataSize & 0x8000) == 0x8000) {
                    stream.MoveBitPosition(-33);
                    var hiSize = stream.GetRawShort();
                    dataSize = (ushort)((dataSize & 0x7FFF) | (hiSize << 15));
                }
                stream.MoveBitPosition(-dataSize - 16);
            }
        }

        /// <summary>
        /// Считывает информацию о сущности управления.
        /// </summary>
        /// <param name="stream">Поток с данными</param>
        /// <param name="bitSize">Размер битов</param>
        /// <param name="entity">Тип сущности</param>
        /// <returns>Сущность</returns>
        public Entity DecodeContol(DwtStream stream, int bitSize, Entity entity)
        {
            // R2000+:
            entity.Size = (uint)(stream.Size() * 8 - bitSize);
            // Common:
            entity.Handle = new ObjectHandle(stream.GetHandle().ToTuple());

            // Чтение расширенных данных.
            var sizeExtended = stream.GetBitShort();
            while (sizeExtended > 0)
                sizeExtended = DecodeExtendedObjectData(stream, sizeExtended);

            entity.NumberReactor = stream.GetBitShort();
            // R2000+:
            entity.XDictFlag = Convert.ToBoolean(stream.GetBit());
            Logger.Debug($"xDictFlag:      {entity.XDictFlag}");
            // R2010+:
            Logger.Debug($"Have binary:    {Convert.ToBoolean(stream.GetBit())}");
            Logger.Debug($"Размер:         {entity.Size}");
            Logger.Debug($"Handle:         {entity.Handle.code}, {entity.Handle.size}, 0x{entity.Handle.reference:X}");
            Logger.Debug($"NumReactors:    {entity.NumberReactor}");

            return entity;
        }

        /// <summary>
        /// Считывает расширенные данные сущности.
        /// </summary>
        /// <param name="stream">Поток с данными</param>
        /// <param name="sizeExtended">Размер расширенных данных</param>
        /// <returns>Новый размер расширенных данных</returns>
        private ushort DecodeExtendedObjectData(DwtStream stream, ushort sizeExtended)
        {
            var handle = stream.GetHandle().ToTuple();
            Logger.Debug($"Handle          {handle.Item1}, {handle.Item2}, 0x{handle.Item3:X}");
            var buffer = new DwtStream(stream.GetBytes(sizeExtended));

            var dxfCode = buffer.GetRawChar();
            Logger.Debug($"DxfCode:        {dxfCode}");

            if (dxfCode == 0) {
                var lenght = buffer.GetRawChar();
                Logger.Debug($"StrLength:      {lenght}");
                Logger.Debug($"StrCodepage:    {buffer.GetBeRawShort()}");

                for (var i = 0; i < lenght; ++i)
                    Logger.Debug($"DxfChar:        {buffer.GetRawChar()}");
            }
            sizeExtended = stream.GetBitShort();
            Logger.Debug($"Размер данных:  {sizeExtended}");

            return sizeExtended;
        }

        /// <summary>
        /// Считывает данные об изображении.
        /// </summary>
        /// <param name="stream">Поток с данными</param>
        private void DecodeGraphicImage(DwtStream stream)
        {
            Logger.Error("Декодирование graph данных отсутствует!");
            // R2010+:
            // Common:
        }
    }
}
