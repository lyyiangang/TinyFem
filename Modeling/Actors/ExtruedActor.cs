using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kitware.mummy;
using Kitware.VTK;
using TinyFem.Base;
using TinyFem.FemElement;
using TinyFem.VTKForm;
namespace TinyFem.Modeling
{
    /// <summary>
    /// 旋转体对象(未剖分以前)
    /// </summary>
    public class ExtruedActor : FEMActor, IFemPart
    {
        ModelingBaseInfo m_modelBaseInfo;
        bool m_selected = false;
        ModelType m_modelType = ModelType.None;
       // double[] m_defaultColor;
        Part m_belongPart = null;
        eActorType m_actorType = eActorType.ViewActor;
        bool m_isSelected = false;
        public ExtruedActor(ModelType type, ModelingBaseInfo baseinfo)
        {
            m_modelType = type;
            m_modelBaseInfo = baseinfo;
            vtkPolyData polydata= ModelingFactory.CreateModelingInstance(type, baseinfo);
            ConstructActor(polydata);
        }
        /// <summary>
        /// 拉伸对象的基本信息
        /// </summary>
       public  ModelingBaseInfo BaseInfo
        {
            get { return m_modelBaseInfo; }
        }
        public  bool Selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }
        /// <summary>
        /// 模型类型
        /// </summary>
        public ModelType ModelType
        {
            get { return m_modelType; }
        }

        void ConstructActor(vtkPolyData polydata)
        {
            //vtkUnsignedCharArray colors = vtkUnsignedCharArray.New();
            //colors.SetNumberOfComponents(3);

            //Console.WriteLine("pointnum:{0},linenum:{1},numPieces:{2},numstrips:{3}", polydata.GetNumberOfPoints(),polydata .GetNumberOfLines()
            //    ,polydata .GetNumberOfPieces(),polydata .GetNumberOfPolys(),polydata .GetNumberOfStrips());

            //int cellnum = polydata.GetNumberOfCells();
            //colors.SetNumberOfTuples(cellnum);
            //for (int i = 0; i < cellnum; ++i)
            //    colors.SetTuple3(i, m_defaultColor[0], m_defaultColor[1], m_defaultColor[2]);
            //polydata.GetCellData().SetScalars(colors);

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
           // mapper.SetScalarModeToUseCellData();
            mapper.SetInput(polydata);
           // float[] color = { 23f, 23f, 100f };
            m_defaultColor = ModelUtils.GetRandomColor01();//获取随机颜色
            SetColor(m_defaultColor);
            this.SetMapper(mapper);
            
        }

        #region IFemActor 成员

        public Part BelongPart
        {
            get
            {
                return m_belongPart;
            }
            set
            {
                m_belongPart = value;
            }
        }

        public eActorType ActorType
        {
            get
            {
                return m_actorType;
            }
            set
            {
                m_actorType = value;
            }
        }

        #endregion

        #region ISelectResonser 成员

        public override  void ClearAllSelected()
        {//清除被选中的颜色 （恢复到默认值）
            this.GetProperty().SetColor(m_defaultColor[0], m_defaultColor[1], m_defaultColor[2]);//设置被选中时的颜色
        }

        public override void AddPointId(int id, ePickBollean boolean)
        {
           // throw new NotImplementedException();
        }

        public override void AddCellId(int id, ePickBollean boolean)
        {
            //throw new NotImplementedException();
        }

        public override void AddPointId(vtkIdTypeArray ids, ePickBollean boolean)
        {
           //throw new NotImplementedException();
        }

        public override void AddCellId(vtkIdTypeArray ids, ePickBollean boolean)
        {
          // throw new NotImplementedException();
        }
        public override void UpdateSelectedColor()
        {
            //if(m_isSelected )
            //    this.GetProperty().SetColor(m_defaultColor[0], m_defaultColor[1], m_defaultColor[2]);//设置被选中时的颜色
                
        }
        public override bool IsSelected
        {
            get
            {
                return m_isSelected;
            }
            set
            {
                if (value)
                {//如果被选中
                    double[] color = ModelUtils.SelectedColor;
                    this.GetProperty().SetColor(color[0], color[1], color[2]);//设置被选中时的颜色
                }
                else//没有选中则恢复到默认颜色
                    this.GetProperty().SetColor(m_defaultColor[0], m_defaultColor[1], m_defaultColor[2]);
                m_isSelected = value;

            }
        }
        ePickBollean m_booleanOperation = ePickBollean.None;
        public override ePickBollean BooleanOperation
        {//不用做布尔操作的。
            get
            {
                return m_booleanOperation;
            }
            set
            {
                m_booleanOperation = value;
            }
        }

        #endregion


    }
}
