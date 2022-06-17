//-----------------------------------------------------------------------------
// <copyright file="TextDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика текста.
// </summary>
//-----------------------------------------------------------------------------

using DwtReader.Components;
using DwtReader.Core;
using DwtReader.ObjectDecoders.Core;
using DwtReader.Objects;
using DwtReader.Objects.Componets;
using DwtReader.Objects.Componets.Text;

using Usl;

namespace DwtReader.ObjectDecoders
{
    /// <summary>
    /// Декодировщик текста.
    /// </summary>
    internal class TextDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"--------------- Text ---------------");

            var stringBuffer = (DwtStream)stream.Clone();
            EntityDecoder entityDecoder = new EntityDecoder();
            var text = (Text)entityDecoder.Decode(stream, bitSize, new Text());

            entityDecoder.NormalizationStringStream(stringBuffer, text.Size);
            text = CommonTextEntityData(stream, stringBuffer, text);
            // Common:
            text = (Text)entityDecoder.DecodeOwner(stream, text);

            Logger.Trace($"CRC:            {stream.GetRawShort()}");
            return text;
        }

        public Text CommonTextEntityData(DwtStream stream, DwtStream stringBuffer, Text text)
        {

            text.Type = ObjectType.Text;

            // R2000+:
            byte dataFlag = stream.GetRawChar();

            if (!((dataFlag & 0x01) > 0))
                text.Elevation = stream.GetRawDouble();

            text.Insertion = (stream.GetRawDouble(), stream.GetRawDouble());

            if (!((dataFlag & 0x02) > 0))
                text.Alignment = (stream.GetDefaultDouble(text.Insertion.x),
                                  stream.GetDefaultDouble(text.Insertion.y));

            text.Thickness = stream.GetBitThickness(true);
            text.Extrusion = stream.GetBitExtrusion(true);

            if (!((dataFlag & 0x04) > 0))
                text.ObliqueAngle = stream.GetRawDouble();
            if (!((dataFlag & 0x08) > 0))
                text.RotationAngle = stream.GetRawDouble();
            text.Height = stream.GetRawDouble();
            if (!((dataFlag & 0x10) > 0))
                text.WidthFactor = stream.GetRawDouble();

            text.Value = stringBuffer.GetVariableText(DwtVersion.AC1027, false).TrimEnd('\0');
            if (!((dataFlag & 0x20) > 0))
                text.Generation = (MirrorFlag)stream.GetBitShort();
            if (!((dataFlag & 0x40) > 0))
                text.HorizontalAlignment = (HorizontalAlignment)stream.GetBitShort();
            if (!((dataFlag & 0x80) > 0))
                text.VerticalAlignment = (VerticalAlignment)stream.GetBitShort();

            Logger.Debug($"Elevation :     {text.Elevation}");
            Logger.Trace($"Insertion x:    {text.Insertion.x:f2}");
            Logger.Trace($"Insertion y:    {text.Insertion.y:f2}");
            Logger.Trace($"Alignment x:    {text.Alignment.x:f2}");
            Logger.Trace($"Alignment y:    {text.Alignment.y:f2}");
            Logger.Debug($"Thickness:      {text.Thickness}");
            Logger.Debug($"Extrusion:      {text.Extrusion}");
            Logger.Debug($"Oblique ang:    {text.ObliqueAngle:f2}");
            Logger.Debug($"Rotation ang:   {text.RotationAngle:f2}");
            Logger.Debug($"Height:         {text.Height}");
            Logger.Debug($"Width factor:   {text.WidthFactor}");
            Logger.Trace($"Text:           {text.Value}");
            Logger.Trace($"Generation:     {text.Generation}");
            Logger.Trace($"Horiz align:    {text.HorizontalAlignment}");
            Logger.Trace($"Vert align:     {text.VerticalAlignment}");
            
            return text;
        }
    }
}
