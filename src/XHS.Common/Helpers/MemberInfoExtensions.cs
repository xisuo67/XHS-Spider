using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XHS.Common.Helpers
{
    /// <summary>
    /// 成员<see cref="MemberInfo"/>的扩展辅助操作方法
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// 获取成员元数据的Description特性描述信息。
        /// </summary>
        /// <param name="member">成员元数据对象。</param>
        /// <param name="inherit">是否搜索成员的继承链以查找描述特性。</param>
        /// <returns>返回Description特性描述信息，如不存在则返回成员的名称。</returns>
        public static string GetDescription(this MemberInfo member, bool inherit = true)
        {
            var desc = member.GetAttribute<DescriptionAttribute>(inherit);
            if (desc != null)
            {
                return desc.Description;
            }

            var displayName = member.GetAttribute<DisplayNameAttribute>(inherit);
            if (displayName != null)
            {
                return displayName.DisplayName;
            }

            var display = member.GetAttribute<DisplayAttribute>(inherit);
            return display != null ? display.Name : member.Name;
        }

        /// <summary>
        /// 检查指定指定类型成员中是否存在指定的Attribute特性。
        /// </summary>
        /// <typeparam name="T">要检查的Attribute特性类型。</typeparam>
        /// <param name="memberInfo">要检查的类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>是否存在</returns>
        public static bool HasAttribute<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            return memberInfo.IsDefined(typeof(T), inherit);
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), inherit);
            return attributes.FirstOrDefault() as T;
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性。
        /// </summary>
        /// <typeparam name="T">Attribute特性类型。</typeparam>
        /// <param name="memberInfo">类型类型成员。</param>
        /// <param name="inherit">是否从继承中查找。</param>
        /// <returns>返回所有指定Attribute特性的数组。</returns>
        public static T[] GetAttributes<T>(this MemberInfo memberInfo, bool inherit = true) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }

        #region 枚举
        /// <summary>
        /// 扩展方法,获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }

        /// <summary>
        /// 扩展方法,获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstead">当枚举值没有定义DescriptionAttribute,是否使用枚举名代替,默认是使用</param>
        /// <returns>枚举的Description</returns>
        public static Dictionary<int, string> GetDescriptionDict(Type enumType)
        {
            return EnumToDictionary(enumType, GetDescription);
        }

        /// <summary>
        /// 把枚举转换为键值对集合
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="getText">获得值得文本</param>
        /// <returns>以枚举值为key,枚举文本为value的键值对集合</returns>
        public static Dictionary<int, string> EnumToDictionary(Type enumType, Func<Enum, string> getText)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");
            }
            Dictionary<int, string> enumDic = new Dictionary<int, string>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum enumValue in enumValues)
            {
                int key = Convert.ToInt32(enumValue);
                string value = getText(enumValue);
                enumDic.Add(key, value);
            }
            return enumDic;
        }

        /// <summary>
        /// 将整型值转换成相应的枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">整形值</param>
        /// <returns>枚举</returns>
        public static T IntToEnum<T>(int value) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!Enum.IsDefined(enumType, value))
            {
                throw new ArgumentException("整形值在相应的枚举里面未定义！");
            }

            return (T)Enum.ToObject(enumType, value);
        }

        /// <summary>
        /// 将枚举转换成相应的整型值
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>整形</returns>
        public static int ToInt(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// 将枚举转换成相应的整型值
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>string</returns>
        public static string ToIntString(this Enum value)
        {
            return Convert.ToInt32(value).ToString();
        }


        /// <summary>
        /// 将整型值转换成相应的枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">整形值</param>
        /// <returns>枚举</returns>
        public static T StringToEnum<T>(string value) where T : struct, IConvertible
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        #endregion
    }
}
