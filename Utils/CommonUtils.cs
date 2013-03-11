using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyFem.Utils
{
    public class FloatCoordinate
    {
        private float x;
        private float y;
        private float z;
        public FloatCoordinate() { }
        public FloatCoordinate(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// X坐标
        /// </summary>
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// Y坐标
        /// </summary>
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// Z坐标
        /// </summary>
        public float Z
        {
            get { return z; }
            set { z = value; }
        }
        /// <summary>
        /// 字符串输入格式为"X;Y;Z"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return x.ToString() + ";" + y.ToString() + ";" + z.ToString();
        }

        /// <summary>
        /// 从字符串中加载坐标，坐标格式"X;Y;Z"
        /// </summary>
        /// <param name="coordinateStr"></param>
        /// <returns></returns>
        public bool LoadFromString(string coordinateStr)
        {
            try
            {
                if (coordinateStr == null || coordinateStr.Length < 2)
                    throw new Exception("字符串格式不正确!");
                string[] tempStr = coordinateStr.Split(new char[] { ';' });
                if (tempStr.Length != 3)
                    throw new Exception("字符串格式不正确!");
                this.x = float.Parse(tempStr[0] != "" ? tempStr[0] : "0");
                this.y = float.Parse(tempStr[1] != "" ? tempStr[1] : "0");
                this.z = float.Parse(tempStr[2] != "" ? tempStr[2] : "0");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class CommonUtils
    {
        
    }
}
