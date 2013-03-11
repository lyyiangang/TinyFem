using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyFem.Modeling
{
    /// <summary>
    /// 矩形靶体的建模参数
    /// </summary>
    public class RectTargetInfo : ModelingBaseInfo
    {
        private float length;//长度
        private float width;
        private float thickness;//厚度

        /// <summary>
        /// 长度
        /// </summary>
        public float Length
        {
            get { return length; }
            set { length = value; }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        public float Width
        {
            get { return width; }
            set { width = value; }
        }
        /// <summary>
        /// 厚度
        /// </summary>
        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
    }
}
