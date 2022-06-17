//-----------------------------------------------------------------------------
// <copyright file="BlockHeaderDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика заголовка блока.
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
    /// Декодировщик заголовка блока.
    /// </summary>
    internal class BlockHeaderDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"----------< Block header >----------");

            // Common:
            var stringBuffer = (DwtStream)stream.Clone();
            EntityDecoder entityDecoder = new EntityDecoder();
            var block = (BlockHeader)entityDecoder.DecodeContol(stream, bitSize, new BlockHeader());

            block.Type = ObjectType.BlockHeader;
            // Common:
            entityDecoder.NormalizationStringStream(stringBuffer, block.Size);
            block.Name = stringBuffer.GetVariableText(DwtVersion.AC1027, false).TrimEnd('\0');

            block.Flag = stream.GetBit() << 0x06;
            block.Flag |= stream.GetBit() << 0x04; // Если блок зависит от внешней ссылки.
            block.Flag |= stream.GetBit() << 0x00; // Если это анонимный блок.
            block.Flag |= stream.GetBit() << 0x01; // Если блок содержит attdefs.
            block.Flag |= stream.GetBit() << 0x02; // Если блок является внешней ссылкой.
            block.Flag |= stream.GetBit() << 0x03; // Если внешняя ссылка наложена.
            // R2000+:
            block.Flag |= stream.GetBit() << 0x05; // Если загружено для внешней ссылки.
            // R2004+:
            block.ObjectCount = stream.GetBitLong();
            // Common:
            block.X = stream.GetBitDouble();
            block.Y = stream.GetBitDouble();
            block.Z = stream.GetBitDouble();
            Logger.Debug($"Xref path name: {stringBuffer.GetVariableText(DwtVersion.AC1027, false)}");
            // R2000+:
            int i = stream.GetRawChar();
            uint insertCount = default;
            while (i != 0) {
                insertCount += (byte)i;
                i = stream.GetRawChar();
            }

            Logger.Debug($"Block descript: {stringBuffer.GetVariableText(DwtVersion.AC1027, false)}");
            // Размер данных предварительного просмотра.
            var prevData = stream.GetBitLong();
            for (i = 0; i < prevData && stream.GetPosition() < stream.Size() - 1; ++i)
                stream.GetRawChar();
            // R2007+:
            Logger.Debug($"Вставные блоки: {stream.GetBitShort()}");
            Logger.Debug($"Explodable:     {Convert.ToBoolean(stream.GetBit())}");
            Logger.Debug($"Масштаб:        {stream.GetRawChar()}");
            // Common:
            stream.SetPosition(block.Size >> 3);
            stream.SetBitPosition((byte)(block.Size & 7));

            var handle = stream.GetHandle();
            Logger.Debug($"BC handle:      {handle.code}.{handle.size}.0x{handle.reference:X}");
            for (i = 0; i < block.NumberReactor; ++i) {
                handle = stream.GetHandle();
                Logger.Debug($"Reactor {i}:    {handle.code}.{handle.size}.0x{handle.reference:X}");
            }

            if (block.XDictFlag == false) {
                handle = stream.GetHandle();
                Logger.Debug($"Xdicobj handle: {handle.code}.{handle.size}.0x{handle.reference:X}");
            }
            handle = stream.GetHandle();
            Logger.Debug($"Null handle:    {handle.code}.{handle.size}.0x{handle.reference:X}");
            handle = stream.GetHandle();
            Logger.Debug($"Block entity:   {handle.code}.{handle.size}.0x{handle.reference:X}");
            // R2004+:
            for (i = 0; i < block.ObjectCount; ++i) {
                block.Handles.Add(new ObjectHandle(stream.GetHandle().ToTuple()));
                Logger.Debug($"Entity handle:  {block.Handles[i].code}.{block.Handles[i].size}.0x{block.Handles[i].reference:X}");
            }
            // Common:
            handle = stream.GetHandle();
            Logger.Debug($"End Block:      {handle.code}.{handle.size}.0x{handle.reference:X}");
            // R2000+:
            for (i = 0; i < insertCount; ++i) {
                handle = stream.GetHandle();
                Logger.Debug($"Insert handle:  {handle.code}.{handle.size}.0x{handle.reference:X}");
            }
            handle = stream.GetHandle();
            Logger.Debug($"Layout handle:  {handle.code}.{handle.size}.0x{handle.reference:X}");
            // Common:
            Logger.Trace($"Name:           {block.Name}");
            Logger.Trace($"Coord:          {block.X}, {block.Y}, {block.Z}");
            Logger.Debug($"Флаг:           0x{block.Flag:X}");
            Logger.Debug($"ObjectCount:    {block.ObjectCount}");
            Logger.Trace($"CRC:            {stream.GetRawShort()}");

            return block;
        }
    }
}
