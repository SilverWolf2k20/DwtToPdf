//-----------------------------------------------------------------------------
// <copyright file="LayerControlDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика слоя управления.
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
    /// Декодировщик слоя управления.
    /// </summary>
    internal class LayerControlDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"---------< Layer control >----------");

            // Common:
            EntityDecoder entityDecoder = new EntityDecoder();
            var layer = (LayerControl)entityDecoder.DecodeContol(stream, bitSize, new LayerControl());

            layer.Type = ObjectType.LayerControl;

            // Common:
            layer.NumberEntries = stream.GetBitLong();
            Logger.Debug($"NumEntries:     {layer.NumberEntries}");
            Logger.Debug($"Биты строк:     {stream.GetBit()}");
            Logger.Debug($"Null handle:    {stream.GetHandle().ToTuple()}");
            // R2004+:
            if (layer.XDictFlag == false)
                Logger.Debug($"XDicObj handle  {stream.GetHandle().ToTuple()}");
            // Common:
            for (int i = 0; i < layer.NumberEntries; ++i) {
                layer.Handles.Add(new ObjectHandle(stream.GetOffsetHandle(layer.Handle.reference).ToTuple()));
                var handle = layer.Handles[i];
                Logger.Trace($"Object handle:  {handle.code}.{handle.size}.0x{handle.reference:X}");
            }

            Logger.Trace($"CRC:            {stream.GetRawShort()}");
            return layer;
        }
    }
}
