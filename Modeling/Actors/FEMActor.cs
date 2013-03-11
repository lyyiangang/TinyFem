using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kitware.mummy;
using Kitware.VTK;
using TinyFem.Base;
using TinyFem.VTKForm;
namespace TinyFem.Modeling
{
    public abstract class FEMActor : vtkActor,ISelectResonser
    {
        protected double[] m_defaultColor;
        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="rgb"></param>
        public void SetColor(double[] rgb)
        {
            if (rgb.Length == 3)
            {
                //m_defaultColor = rgb;//保存为默认值
                this.GetProperty().SetColor(rgb[0], rgb[1], rgb[2]);
            }
        }
        #region ISelectResonser 成员

        public abstract void ClearAllSelected();

        public abstract void  AddPointId(int id,ePickBollean boolean);

        public abstract void AddCellId(int id, ePickBollean boolean);

        public abstract void AddPointId(vtkIdTypeArray ids, ePickBollean boolean);

        public abstract void AddCellId(vtkIdTypeArray ids, ePickBollean boolean);

        public virtual  bool IsSelected
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual ePickBollean BooleanOperation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public abstract void UpdateSelectedColor();

        #endregion
    }
}
