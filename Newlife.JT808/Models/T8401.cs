using NewLife.JT808.Protocols;
using NewLife.Serialization;

namespace NewLife.JT808.Models
{
    /// <summary>设置电话本 设置类型</summary>
    public enum SetTelephoneBook : byte
    {
        /// <summary>删除终端上所有存储的联系人</summary>
        删除终端上所有存储的联系人 = 0,

        /// <summary>更新电话本_删除终端中已有全部联系人并追加消息中的联系人</summary>
        更新电话本_删除终端中已有全部联系人并追加消息中的联系人 = 1,

        /// <summary>追加电话本</summary>
        追加电话本 = 2,

        /// <summary>修改电话本_以联系人为索引</summary>
        修改电话本_以联系人为索引 = 3
    }

    /// <summary>设置电话本</summary>
    [MessageKind(MessageKinds.设置电话本)]
    public class T8401 : IAccessor
    {
        /// <summary>设置类型</summary>
        public SetTelephoneBook SettingTelephoneBook { get; set; }

        /// <summary>联系人总数</summary>
        public Byte ContactCount { get; set; }

        /// <summary>联系人项</summary>
        public IList<ContactProperty> JT808ContactProperties { get; set; }

        /// <summary>读取</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Read(Stream stream, Object? context)
        {
            var reader = Helper.CreateReader(stream, context as JTMessage);

            SettingTelephoneBook = (SetTelephoneBook)reader.ReadByte();
            ContactCount = reader.ReadByte();

            var list = new List<ContactProperty>();
            for (var i = 0; i < ContactCount; i++)
            {
                var item = new ContactProperty
                {
                    Type = (ContactType)reader.ReadByte(),
                    PhoneNumberLength = reader.ReadByte(),
                };
                item.PhoneNumber = reader.ReadFixedString(item.PhoneNumberLength);
                item.ContactLength = reader.ReadByte();
                item.Contact = reader.ReadFixedString(item.ContactLength);

                list.Add(item);
            }
            JT808ContactProperties = list;

            return true;
        }

        /// <summary>写入</summary>
        /// <param name="stream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Boolean Write(Stream stream, Object? context)
        {
            var writer = Helper.CreateWriter(stream, context as JTMessage);

            var items = JT808ContactProperties ?? [];
            ContactCount = (Byte)items.Count;

            writer.Write((Byte)SettingTelephoneBook);
            writer.Write(ContactCount);

            foreach (var item in items)
            {
                writer.Write((Byte)item.Type);

                var phones = Helper.Encoding.GetBytes(item.PhoneNumber ?? String.Empty);
                writer.Write((Byte)phones.Length);
                if (phones.Length > 0) writer.Write(phones, 0, phones.Length);

                var contacts = Helper.Encoding.GetBytes(item.Contact ?? String.Empty);
                writer.Write((Byte)contacts.Length);
                if (contacts.Length > 0) writer.Write(contacts, 0, contacts.Length);
            }

            return true;
        }
    }
}