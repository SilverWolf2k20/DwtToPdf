//-----------------------------------------------------------------------------
// <copyright file="BlockControlDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика блока управления.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Core;
using DwtReader.ObjectDecoders.Core;
using DwtReader.Objects;
using DwtReader.Objects.Componets;
using DwtReader.Objects.Controls;

using Usl;

namespace DwtReader.ObjectDecoders.ControlsDecoders
{
    /// <summary>
    /// Декодировщик блока управления.
    /// </summary>
    internal class BlockControlDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"---------< Block control >----------");

            // Common:
            EntityDecoder entityDecoder = new EntityDecoder();
            var block = (BlockControl)entityDecoder.DecodeContol(stream, bitSize, new BlockControl());

            block.Type = ObjectType.BlockControl;
            // Common:
            block.NumberEntries = stream.GetBitLong();
            Logger.Debug($"NumEntries:     {block.NumberEntries}");
            Logger.Debug($"Биты строк:     {stream.GetBit()}");
            Logger.Debug($"Null handle:    {stream.GetHandle().ToTuple()}");
            // R2004+:
            if (block.XDictFlag == false)
                Logger.Debug($"XDicObj handle  {stream.GetHandle().ToTuple()}");
            // Common:
            block.NumberEntries += 2;

            for (int i = 0; i < block.NumberEntries; ++i) {
                block.Handles.Add(new ObjectHandle(stream.GetOffsetHandle(block.Handle.reference).ToTuple()));
                var handle = block.Handles[i];
                Logger.Trace($"Object handle:  {handle.code}.{handle.size}.0x{handle.reference:X}");
            }

            Logger.Trace($"CRC:            {stream.GetRawShort()}");
            return block;
        }
    }
}
