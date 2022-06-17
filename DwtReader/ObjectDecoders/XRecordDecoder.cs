//-----------------------------------------------------------------------------
// <copyright file="XRecordDecoder.cs" company="SW Okolo IT">
// Copyright (c) SW Okolo IT. All rights reserved.
// </copyright>
// <summary>
// Содержит класс декодировщика хранилища данных.
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
    /// 
    /// </summary>
    public enum CodeValueType : byte
    {
        None,
        Text,
        Point3D,
        Double,
        Short,
        Long,
        LongLong,
        Handle,
        Bit,
        Char,
        Comments
    }

    /// <summary>
    /// Декодировщик хранилища данных.
    /// </summary>
    internal class XRecordDecoder : IEntityDecoder
    {
        public Entity Decode(DwtStream stream, int bitSize, Entity entity)
        {
            Logger.Info($"-------------- XRecord -------------");

            EntityDecoder entityDecoder = new EntityDecoder();
            var xRecord = (XRecord)entityDecoder.DecodeContol(stream, bitSize, new XRecord());

            xRecord.Type = ObjectType.XRecord;

            // Common:
            var offset = stream.GetPosition() + stream.GetBitLong();
            Logger.Debug($"Offset:         {offset}");

            /*
            var clone = (DwtStream)stream.Clone();
            clone.SetPosition(stream.GetPosition());
            clone.SetBitPosition(stream.GetBitPosition());
            for (int i = 0; clone.GetPosition() < offset; ++i) {
                var data = clone.GetRawChar();
                Console.WriteLine($"Pos: {clone.GetPosition()}\t({data:X2}\t{(char)data})");
            }
            Console.WriteLine();
            */

            /*
            while (stream.GetPosition() < offset) {
                var code = stream.GetRawShort();

                switch (TransformValue(code)) {
                    case CodeValueType.Bit:
                        Logger.Debug($"Bit:            {stream.GetRawChar()} code: {code}");
                        break;
                    case CodeValueType.Double:
                        Logger.Debug($"Double:         {stream.GetRawDouble()} code: {code}");
                        break;
                    case CodeValueType.Char:
                        stream.GetBytes(stream.GetRawChar());
                        Logger.Debug($"Chank:          code: {code}");
                        break;
                    case CodeValueType.Short:
                        Logger.Debug($"Short:          {stream.GetRawShort()} code: {code}");
                        break;
                    case CodeValueType.Long:
                        Logger.Debug($"Long:           {stream.GetRawLong()} code: {code}");
                        break;
                    case CodeValueType.LongLong:
                        Logger.Debug($"LongLong:       {stream.GetRawLong()} code: {code}");
                        break;
                    case CodeValueType.Text:
                        Logger.Debug($"Text:           {GetText(stream)} code: {code}");
                        break;
                    case CodeValueType.Point3D:
                        Logger.Debug($"Point3D x:      {stream.GetRawDouble()} code: {code}");
                        Logger.Debug($"Point3D y:      {stream.GetRawDouble()} code: {code}");
                        Logger.Debug($"Point3D z:      {stream.GetRawDouble()} code: {code}");
                        break;
                    case CodeValueType.Handle:
                        Logger.Debug($"Handle:         {GetText(stream)} code: {code}");
                        break;
                    default:
                        break;
                }
            }
            */
            // R2000+:
            // Cloning flag:

            return xRecord;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private CodeValueType TransformValue(int code)
        {
            if (code > 1071)
                return CodeValueType.None;

            if (code == 5 || code == 105)
                return CodeValueType.Handle;
            if (code >= 100 && code <= 102)
                return CodeValueType.Text;
            if (code == 999)
                return CodeValueType.Comments;
            if (code == 1004)  // ExtendedDataChar
                return CodeValueType.None;
            if (code == 1071)  // ExtendedDataLong
                return CodeValueType.None;

            if (code >= 0 && code <= 9)
                return CodeValueType.Text;
            if (code >= 10 && code <= 39)
                return CodeValueType.Point3D;
            if (code >= 40 && code <= 59)
                return CodeValueType.Double;
            if (code >= 60 && code <= 79)
                return CodeValueType.Short;
            if (code >= 90 && code <= 99)

                return CodeValueType.Long;
            if (code >= 110 && code <= 149)
                return CodeValueType.Double;
            if (code >= 160 && code <= 169)
                return CodeValueType.LongLong;
            if (code >= 170 && code <= 179)

                return CodeValueType.Short;
            if (code >= 210 && code <= 239)
                return CodeValueType.Double;
            if (code >= 270 && code <= 289)
                return CodeValueType.Short;
            if (code >= 290 && code <= 199)
                return CodeValueType.Bit;

            if (code >= 300 && code <= 309)
                return CodeValueType.Text;
            if (code >= 310 && code <= 319)
                return CodeValueType.Char;
            if (code >= 320 && code <= 329)
                return CodeValueType.Handle;
            if (code >= 330 && code <= 369)
                return CodeValueType.None; // ObjectID
            if (code >= 370 && code <= 389)
                return CodeValueType.Short;
            if (code >= 390 && code <= 399)
                return CodeValueType.Handle;

            if (code >= 400 && code <= 409)
                return CodeValueType.Short;
            if (code >= 410 && code <= 419)
                return CodeValueType.Text;
            if (code >= 420 && code <= 429)
                return CodeValueType.Long;
            if (code >= 430 && code <= 439)
                return CodeValueType.Text;
            if (code >= 440 && code <= 459)
                return CodeValueType.Long;
            if (code >= 460 && code <= 469)
                return CodeValueType.Double;
            if (code >= 470 && code <= 479)
                return CodeValueType.Text;
            if (code >= 480 && code <= 481)
                return CodeValueType.Handle;

            if (code >= 1000 && code <= 1003) // ExtendedDataString
                return CodeValueType.None;
            if (code >= 1005 && code <= 1009) // ExtendedDataHandle
                return CodeValueType.None;
            if (code >= 1010 && code <= 1059) // ExtendedDataDouble
                return CodeValueType.None;
            if (code >= 1060 && code <= 1070) // ExtendedDataShort
                return CodeValueType.None;
            return CodeValueType.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private string GetText(DwtStream stream)
        {
            var lenght = stream.GetRawShort();
            var text = "";

            for (int i = 0; i < lenght; ++i) {
                text += (char)stream.GetRawChar();
                stream.GetRawChar();
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private double GetDouble(DwtStream stream)
        {
            byte[] buffer = new byte[8];

            for (int i = 0; i < 8; ++i)
               buffer[i] = stream.GetRawChar();

            return BitConverter.ToDouble(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private int GetChank(DwtStream stream)
        {
            /*
            var size = stream.Read();

            for(var i = 0; i < size; ++i)
                stream.Read();
            */
            return 228;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private short GetShort(DwtStream stream)
        {
            //byte[] buffer = stream.Read(2);
            //return (short)((buffer[1] << 8) | (buffer[0] & 0x00FF));
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private int GetLong(DwtStream stream)
        {
            short first = GetShort(stream);
            short last  = GetShort(stream);
            return (last << 16) | (first & 0x0000FFFF);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private long GetLongLong(DwtStream stream)
        {
            long first = GetLong(stream);
            long last  = GetLong(stream);
            return (first << 32) | (last & 0x00000000FFFFFFFF);
        }
    }
}