using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace TinyFem.Modeling
{
    /// <summary>
    /// VTK中Cell的类型(扩展)
    /// </summary>
    public enum VTKCellType
    {
        VTK_VERTEX = 1,
        VTK_POLY_VERTEX = 2,
        VTK_LINE = 3,
        VTK_POLY_LINE = 4,
        VTK_TRIANGLE = 5,
        VTK_TRANGLE_STRIP = 6,
        VTK_POLYGON = 7,
        VTK_PIXEL = 8,
        VTK_QUAD = 9,
        VTK_TETRA = 10,
        VTK_VOXEL = 11,
        VTK_HEXAHEDRON = 12,
        VTK_WEDGE = 13,
        VTK_PYRAMID = 14,
        VTK_UNKNOWN = 15
    }
   public  class ModelUtils
    {
       static double[] m_selectedColor ={132,60,210 };
        /// <summary>
        /// 选择时的颜色
        /// </summary>
       static public double[]  SelectedColor
        {
            get { return m_selectedColor; }
            set { m_selectedColor = value; }
        }
       /// <summary>
       /// 获取浮点类型0-1的rgb值,property用
       /// </summary>
       /// <returns></returns>
       static public double[] GetRandomColor01()
        {//获取随机颜色
            double[] rgb = new double[3];
            System.Random random = new Random();
            rgb[0] = random.NextDouble();
            rgb[1] = random.NextDouble();
            rgb[2] = random.NextDouble();
            return rgb;
        }
       /// <summary>
       /// 0-255范围的rgb值，变态的vtk，怎么会有两种类型rgb，为cell，points等赋值
       /// </summary>
       /// <returns></returns>
       static public double[] GetRandomColor256()
       {
           double[] color = GetRandomColor01();
           double[] rgb=new double[3];
           for (int i = 0; i < color.Length ; i++)
               rgb[i] = color[i] * 255;
           return rgb;
       }

    }
}
