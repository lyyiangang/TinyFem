using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Drawing;

namespace TinyFem.Utils
{
	interface ISerialize
	{   
        /// <summary>
        /// 通过这个接口可以对所有对象进行序列化
        /// </summary>
        /// <param name="wr"></param>
		void GetObjectData(XmlWriter wr);
		void AfterSerializedIn();
	}
	public class XmlSerializable : System.Attribute
	{
		public XmlSerializable() { }
	}
	class XmlUtil
	{
		public static void AddProperty(string name, object value, XmlWriter wr)
		{
			string svalue = string.Empty;
			if (value is string)
				svalue = value as string;
			if (svalue.Length == 0 && value.GetType() == typeof(float))
				svalue = XmlConvert.ToString(Math.Round((float)value, 8));
			if (svalue.Length == 0 && value.GetType() == typeof(double))
				svalue = XmlConvert.ToString(Math.Round((double)value, 8));
			if (svalue.Length == 0)
				svalue = value.ToString();
			
			wr.WriteStartElement("property");
			wr.WriteAttributeString("name", name);
			wr.WriteAttributeString("value", svalue);
			wr.WriteEndElement();
		}
        /// <summary>
        /// 将xml节点中的数据转换dataobject中的属性，并存储在dataobject中去
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dataobject"></param>
		public static void ParseProperty(XmlElement node, object dataobject,bool nameMustBeProperty)
		{
			if (node.Name != "property"&&nameMustBeProperty )
				return;

			string fieldname = node.GetAttribute("name");
			string svalue = node.GetAttribute("value");
			if (fieldname.Length == 0 || svalue.Length == 0)
				return;

			PropertyInfo info = CommonTools.PropertyUtil.GetProperty(dataobject, fieldname);
		if (info == null || info.CanWrite == false)
				return;	
			try
			{
				object value = PropertyUtil.ChangeType(svalue, info.PropertyType);
				if (value != null)
					info.SetValue(dataobject, value, null);
			}
			catch {};
		}
        public static void ParsePropertyFromAttribute(XmlElement node, object dataobject, string[] attName)
        {//从每个节点的属性中进行赋值
            int length = attName.Length;
            try
            {
                for (int i = 0; i < length; i++)
                {
                    string svalue = node.GetAttribute(attName[i]);
                    if (attName[i].Length == 0 || svalue.Length == 0)
                        continue;
                    PropertyInfo info = CommonTools.PropertyUtil.GetProperty(dataobject, attName[i]);
                    if (info == null || info.CanWrite == false)
                        continue;
                    object value = PropertyUtil.ChangeType(svalue, info.PropertyType);
                    if (value != null)
                        info.SetValue(dataobject, value, null);
                }
            }
            catch { };
        }
        public static void ParseProperties(XmlElement itemnode, object dataobject)
		{
			foreach (XmlElement propertynode in itemnode.ChildNodes)
				XmlUtil.ParseProperty(propertynode, dataobject,true );
		}
		public static void WriteProperties(object dataobject, XmlWriter wr)
		{
			foreach (PropertyInfo propertyInfo in dataobject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
                //获取用户自己定义的属性
				XmlSerializable attr = (XmlSerializable)Attribute.GetCustomAttribute(propertyInfo, typeof(XmlSerializable));
				if (attr != null)
				{
					string name	= propertyInfo.Name;
					object value = propertyInfo.GetValue(dataobject, null);
					if (value != null)
						AddProperty(name, value, wr);
				}
			}
		}
	}

    class PropertyUtil
    {
        public static object ChangeType(object value, Type type)
        {
            throw new Exception("not define");
            //if (type == typeof(UnitPoint))
            //    return Parse(value.ToString(), type);
            //return CommonTools.PropertyUtil.ChangeType(value, type);
        }
        static public object Parse(string value, Type type)
        {
            throw new Exception("not define");
            //if (type == typeof(UnitPoint))
            //    return CommonTools.PropertyUtil.Parse(new UnitPoint(0, 0), value);
            //return CommonTools.PropertyUtil.Parse(value, type);
        }
    }

	class SerializeUtil
	{

	}
}
