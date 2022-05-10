//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------

#region License

// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion License

using System;
using System.IO;

#if HAVE_BIG_INTEGER
using System.Numerics;
#endif

using RRQMCore.XREF.Newtonsoft.Json.Utilities;
using System.Globalization;

namespace RRQMCore.XREF.Newtonsoft.Json.Bson
{
    /// <summary>
    /// Represents a writer that provides a fast, non-cached, forward-only way of generating BSON data.
    /// </summary>
    [Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/RRQMCore.XREF.Newtonsoft.Json.Bson for more details.")]
    public class BsonWriter : JsonWriter
    {
        private readonly BsonBinaryWriter _writer;

        private BsonToken _root;
        private BsonToken _parent;
        private string _propertyName;

        /// <summary>
        /// Gets or sets the <see cref="DateTimeKind" /> used when writing <see cref="DateTime"/> values to BSON.
        /// When set to <see cref="DateTimeKind.Unspecified" /> no conversion will occur.
        /// </summary>
        /// <value>The <see cref="DateTimeKind" /> used when writing <see cref="DateTime"/> values to BSON.</value>
        public DateTimeKind DateTimeKindHandling
        {
            get => this._writer.DateTimeKindHandling;
            set => this._writer.DateTimeKindHandling = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonWriter"/> class.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to write to.</param>
        public BsonWriter(Stream stream)
        {
            ValidationUtils.ArgumentNotNull(stream, nameof(stream));
            this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonWriter"/> class.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
        public BsonWriter(BinaryWriter writer)
        {
            ValidationUtils.ArgumentNotNull(writer, nameof(writer));
            this._writer = new BsonBinaryWriter(writer);
        }

        /// <summary>
        /// Flushes whatever is in the buffer to the underlying <see cref="Stream"/> and also flushes the underlying stream.
        /// </summary>
        public override void Flush()
        {
            this._writer.Flush();
        }

        /// <summary>
        /// Writes the end.
        /// </summary>
        /// <param name="token">The token.</param>
        protected override void WriteEnd(JsonToken token)
        {
            base.WriteEnd(token);
            this.RemoveParent();

            if (this.Top == 0)
            {
                this._writer.WriteToken(this._root);
            }
        }

        /// <summary>
        /// Writes a comment <c>/*...*/</c> containing the specified text.
        /// </summary>
        /// <param name="text">Text to place inside the comment.</param>
        public override void WriteComment(string text)
        {
            throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
        }

        /// <summary>
        /// Writes the start of a constructor with the given name.
        /// </summary>
        /// <param name="name">The name of the constructor.</param>
        public override void WriteStartConstructor(string name)
        {
            throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
        }

        /// <summary>
        /// Writes raw JSON.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        public override void WriteRaw(string json)
        {
            throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
        }

        /// <summary>
        /// Writes raw JSON where a value is expected and updates the writer's state.
        /// </summary>
        /// <param name="json">The raw JSON to write.</param>
        public override void WriteRawValue(string json)
        {
            throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
        }

        /// <summary>
        /// Writes the beginning of a JSON array.
        /// </summary>
        public override void WriteStartArray()
        {
            base.WriteStartArray();

            this.AddParent(new BsonArray());
        }

        /// <summary>
        /// Writes the beginning of a JSON object.
        /// </summary>
        public override void WriteStartObject()
        {
            base.WriteStartObject();

            this.AddParent(new BsonObject());
        }

        /// <summary>
        /// Writes the property name of a name/value pair on a JSON object.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public override void WritePropertyName(string name)
        {
            base.WritePropertyName(name);

            this._propertyName = name;
        }

        /// <summary>
        /// Closes this writer.
        /// If <see cref="JsonWriter.CloseOutput"/> is set to <c>true</c>, the underlying <see cref="Stream"/> is also closed.
        /// If <see cref="JsonWriter.AutoCompleteOnClose"/> is set to <c>true</c>, the JSON is auto-completed.
        /// </summary>
        public override void Close()
        {
            base.Close();

            if (this.CloseOutput)
            {
                this._writer?.Close();
            }
        }

        private void AddParent(BsonToken container)
        {
            this.AddToken(container);
            this._parent = container;
        }

        private void RemoveParent()
        {
            this._parent = this._parent.Parent;
        }

        private void AddValue(object value, BsonType type)
        {
            this.AddToken(new BsonValue(value, type));
        }

        internal void AddToken(BsonToken token)
        {
            if (this._parent != null)
            {
                BsonObject bo = this._parent as BsonObject;
                if (bo != null)
                {
                    bo.Add(this._propertyName, token);
                    this._propertyName = null;
                }
                else
                {
                    ((BsonArray)this._parent).Add(token);
                }
            }
            else
            {
                if (token.Type != BsonType.Object && token.Type != BsonType.Array)
                {
                    throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
                }

                this._parent = token;
                this._root = token;
            }
        }

        #region WriteValue methods

        /// <summary>
        /// Writes a <see cref="Object"/> value.
        /// An error will raised if the value cannot be written as a single JSON token.
        /// </summary>
        /// <param name="value">The <see cref="Object"/> value to write.</param>
        public override void WriteValue(object value)
        {
#if HAVE_BIG_INTEGER
            if (value is BigInteger)
            {
                SetWriteState(JsonToken.Integer, null);
                AddToken(new BsonBinary(((BigInteger)value).ToByteArray(), BsonBinaryType.Binary));
            }
            else
#endif
            {
                base.WriteValue(value);
            }
        }

        /// <summary>
        /// Writes a null value.
        /// </summary>
        public override void WriteNull()
        {
            base.WriteNull();
            this.AddToken(BsonEmpty.Null);
        }

        /// <summary>
        /// Writes an undefined value.
        /// </summary>
        public override void WriteUndefined()
        {
            base.WriteUndefined();
            this.AddToken(BsonEmpty.Undefined);
        }

        /// <summary>
        /// Writes a <see cref="String"/> value.
        /// </summary>
        /// <param name="value">The <see cref="String"/> value to write.</param>
        public override void WriteValue(string value)
        {
            base.WriteValue(value);
            this.AddToken(value == null ? BsonEmpty.Null : new BsonString(value, true));
        }

        /// <summary>
        /// Writes a <see cref="Int32"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Int32"/> value to write.</param>
        public override void WriteValue(int value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Integer);
        }

        /// <summary>
        /// Writes a <see cref="UInt32"/> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt32"/> value to write.</param>

        public override void WriteValue(uint value)
        {
            if (value > int.MaxValue)
            {
                throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
            }

            base.WriteValue(value);
            this.AddValue(value, BsonType.Integer);
        }

        /// <summary>
        /// Writes a <see cref="Int64"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Int64"/> value to write.</param>
        public override void WriteValue(long value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Long);
        }

        /// <summary>
        /// Writes a <see cref="UInt64"/> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt64"/> value to write.</param>

        public override void WriteValue(ulong value)
        {
            if (value > long.MaxValue)
            {
                throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
            }

            base.WriteValue(value);
            this.AddValue(value, BsonType.Long);
        }

        /// <summary>
        /// Writes a <see cref="Single"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Single"/> value to write.</param>
        public override void WriteValue(float value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Number);
        }

        /// <summary>
        /// Writes a <see cref="Double"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Double"/> value to write.</param>
        public override void WriteValue(double value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Number);
        }

        /// <summary>
        /// Writes a <see cref="Boolean"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Boolean"/> value to write.</param>
        public override void WriteValue(bool value)
        {
            base.WriteValue(value);
            this.AddToken(value ? BsonBoolean.True : BsonBoolean.False);
        }

        /// <summary>
        /// Writes a <see cref="Int16"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Int16"/> value to write.</param>
        public override void WriteValue(short value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Integer);
        }

        /// <summary>
        /// Writes a <see cref="UInt16"/> value.
        /// </summary>
        /// <param name="value">The <see cref="UInt16"/> value to write.</param>

        public override void WriteValue(ushort value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Integer);
        }

        /// <summary>
        /// Writes a <see cref="Char"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Char"/> value to write.</param>
        public override void WriteValue(char value)
        {
            base.WriteValue(value);
            string s = null;
#if HAVE_CHAR_TO_STRING_WITH_CULTURE
            s = value.ToString(CultureInfo.InvariantCulture);
#else
            s = value.ToString();
#endif
            this.AddToken(new BsonString(s, true));
        }

        /// <summary>
        /// Writes a <see cref="Byte"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Byte"/> value to write.</param>
        public override void WriteValue(byte value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Integer);
        }

        /// <summary>
        /// Writes a <see cref="SByte"/> value.
        /// </summary>
        /// <param name="value">The <see cref="SByte"/> value to write.</param>

        public override void WriteValue(sbyte value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Integer);
        }

        /// <summary>
        /// Writes a <see cref="Decimal"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Decimal"/> value to write.</param>
        public override void WriteValue(decimal value)
        {
            base.WriteValue(value);
            this.AddValue(value, BsonType.Number);
        }

        /// <summary>
        /// Writes a <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTime"/> value to write.</param>
        public override void WriteValue(DateTime value)
        {
            base.WriteValue(value);
            value = DateTimeUtils.EnsureDateTime(value, this.DateTimeZoneHandling);
            this.AddValue(value, BsonType.Date);
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Writes a <see cref="DateTimeOffset"/> value.
        /// </summary>
        /// <param name="value">The <see cref="DateTimeOffset"/> value to write.</param>
        public override void WriteValue(DateTimeOffset value)
        {
            base.WriteValue(value);
            AddValue(value, BsonType.Date);
        }
#endif

        /// <summary>
        /// Writes a <see cref="Byte"/>[] value.
        /// </summary>
        /// <param name="value">The <see cref="Byte"/>[] value to write.</param>
        public override void WriteValue(byte[] value)
        {
            if (value == null)
            {
                this.WriteNull();
                return;
            }

            base.WriteValue(value);
            this.AddToken(new BsonBinary(value, BsonBinaryType.Binary));
        }

        /// <summary>
        /// Writes a <see cref="Guid"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Guid"/> value to write.</param>
        public override void WriteValue(Guid value)
        {
            base.WriteValue(value);
            this.AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
        }

        /// <summary>
        /// Writes a <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> value to write.</param>
        public override void WriteValue(TimeSpan value)
        {
            base.WriteValue(value);
            this.AddToken(new BsonString(value.ToString(), true));
        }

        /// <summary>
        /// Writes a <see cref="Uri"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Uri"/> value to write.</param>
        public override void WriteValue(Uri value)
        {
            if (value == null)
            {
                this.WriteNull();
                return;
            }

            base.WriteValue(value);
            this.AddToken(new BsonString(value.ToString(), true));
        }

        #endregion WriteValue methods

        /// <summary>
        /// Writes a <see cref="Byte"/>[] value that represents a BSON object id.
        /// </summary>
        /// <param name="value">The Object ID value to write.</param>
        public void WriteObjectId(byte[] value)
        {
            ValidationUtils.ArgumentNotNull(value, nameof(value));

            if (value.Length != 12)
            {
                throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
            }

            // hack to update the writer state
            this.SetWriteState(JsonToken.Undefined, null);
            this.AddValue(value, BsonType.Oid);
        }

        /// <summary>
        /// Writes a BSON regex.
        /// </summary>
        /// <param name="pattern">The regex pattern.</param>
        /// <param name="options">The regex options.</param>
        public void WriteRegex(string pattern, string options)
        {
            ValidationUtils.ArgumentNotNull(pattern, nameof(pattern));

            // hack to update the writer state
            this.SetWriteState(JsonToken.Undefined, null);
            this.AddToken(new BsonRegex(pattern, options));
        }
    }
}