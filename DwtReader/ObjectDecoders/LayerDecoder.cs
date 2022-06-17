//-----------------------------------------------------------------------------
// <copyright file="LayerDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика слоя.
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
    /// Декодировщик слоя.
    /// </summary>
    internal class LayerDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"--------------- Layer --------------");

            var stringBuffer = (DwtStream)stream.Clone();
            EntityDecoder entityDecoder = new EntityDecoder();
            var layer = (Layer)entityDecoder.Decode(stream, bitSize, new Layer());

            layer.Type = ObjectType.Layer;

            // Common:
            entityDecoder.NormalizationStringStream(stringBuffer, layer.Size);
            layer.Name = stringBuffer.GetVariableText(DwtVersion.AC1027, false).TrimEnd('\0');

            Logger.Trace($"Name:           {layer.Name}");
            Logger.Debug($"Flag:           {stream.GetBit() << 6}");
            Logger.Debug($"Flag:           {stream.GetBit() << 4}");

            Logger.Debug($"F?:             {stream.GetBitShort()}");

            // TODO: Дописать чтение слоя.
            // 167 страница. drw_objects.cpp 618
            // R2000+:

            // Common:

            // R2000+:

            // R2007+:

            // Common:

            return layer;
        }
    }
}
