using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kitware.mummy;
using Kitware.VTK;
using TinyFem.Base;
using TinyFem.VTKForm;
using System.Diagnostics;
namespace TinyFem.Modeling
{
    /// <summary>
    /// 包含网格信息的对象
    /// </summary>
    public class GridActor : FEMActor
    {
        vtkUnstructuredGrid m_gridData;
        vtkPoints m_points=null ;//节点
        vtkCellArray m_cells=null ;//单元
        vtkCellArray m_verticesCell = null;
        Dictionary<int, bool> m_selectedPointsIds =null ;//用来存储被选择的点或者cell的id
        Dictionary<int, bool> m_selectedCellsIds = null;
        
        //节点编号的映射
        Dictionary<int, int> m_idsNodesFEM2Vtk = new Dictionary<int, int>();//
        Dictionary<int, int> m_idsNodesVtk2FEM = new Dictionary<int, int>();
        //单元编号的映射
        Dictionary<int, int> m_idsEleFEM2Vtk = new Dictionary<int, int>();
        Dictionary<int, int> m_idsEleVtk2FEM = new Dictionary<int, int>();
        //保存当前cell的id
       // static int m_totalCell = 0;
        public GridActor()
        {
            m_gridData = new vtkUnstructuredGrid();
            m_points = new vtkPoints();
            m_cells = new vtkCellArray();
          //  m_totalCell = 0;
        }
        public void AddNode(Node node)
        {
            int index = m_points.InsertNextPoint(node.Cor.X, node.Cor.Y, node.Cor.Z);
            m_idsNodesFEM2Vtk.Add(node.NodeId, index);
            m_idsNodesVtk2FEM.Add(index, node.NodeId );
        }
        /// <summary>
        /// 添加四面体单元
        /// </summary>
        /// <param name="ele"></param>
        public void AddTetraElement(Element ele)
        {
            if (ele.EleType != eEleType.Tetra)
                throw new Exception("该构造函数只能构造四面体单元，请检查输入单元类型");
            vtkTetra teracell = new vtkTetra();
            int index=0;
            foreach (var item in ele.NodeIds)
            {
                if (m_idsNodesFEM2Vtk.ContainsKey(item))
                    teracell.GetPointIds().SetId(index++, m_idsNodesFEM2Vtk[item]);
                else
                {
                    Debug.WriteLine("AddTetraElement: "+item.ToString ());
                }
            }
            int cellid = m_cells.InsertNextCell(teracell);
            m_idsEleFEM2Vtk.Add(ele.EleId, cellid);
            m_idsEleVtk2FEM.Add(cellid, ele.EleId);
        }
        /// <summary>
        /// 根据节点和单元信息来Mapper
        /// </summary>
        public void ConstructActor()
        {
            if (m_cells.GetNumberOfCells() < 0)
                Debug.Fail("m_tetraElements没有cell");
            m_gridData.SetPoints(m_points);
            m_gridData.SetCells((int)VTKCellType.VTK_TETRA, m_cells);

            //AddVertex(m_points, m_gridData);//添加vertex
            //vtkVertexGlyphFilter filter = new vtkVertexGlyphFilter();
            
            //filter.SetInput(m_gridData);

            ////
            vtkDataSetMapper dataSetMapper = new vtkDataSetMapper();
            dataSetMapper.SetInput(m_gridData);
            //dataSetMapper.SetInput(filter.GetOutput ());
            this.SetMapper(dataSetMapper);
            this.GetProperty().SetPointSize(5);
            m_defaultColor = ModelUtils.GetRandomColor256();//获取随机颜色
            SetDefaultColor();
            this.GetProperty().SetEdgeVisibility(1);
        }
        public void SetDefaultColor()
        {
            vtkUnsignedCharArray cellColors = new vtkUnsignedCharArray();
            cellColors.SetNumberOfComponents(3);
            int cellNum = this.GetMapper().GetInput().GetNumberOfCells();
            cellColors.SetNumberOfTuples(cellNum);
            for (int i = 0; i < cellNum; ++i)
                cellColors.SetTuple3(i, this.m_defaultColor[0], this.m_defaultColor[1], this.m_defaultColor[2]);
            this.GetMapper().GetInput().GetCellData().SetScalars(cellColors);

            vtkUnsignedCharArray ptColors = new vtkUnsignedCharArray();
            ptColors.SetNumberOfComponents(3);
            int ptNum = this.GetMapper().GetInput().GetNumberOfPoints();
            ptColors.SetNumberOfTuples(ptNum);
            for(int i=0;i<ptNum ;i++)
                ptColors .SetTuple3(i, this.m_defaultColor[0], this.m_defaultColor[1], this.m_defaultColor[2]);
            this.GetMapper().GetInput().GetPointData().SetScalars(ptColors);
           // this.GetMapper().SetScalarModeToUsePointData();

            this.GetMapper().SetScalarModeToUseCellData();
            this.GetMapper().Update();
        }
        void AddVertex(vtkPoints points ,vtkUnstructuredGrid grid)
        {//在每个points的位置上填上一个vertex
            //m_verticesCell = new vtkCellArray();
            //for (int i = 0; i < points.GetNumberOfPoints(); i++)
            //{
            //    vtkVertex vertex = new vtkVertex();
                
            //    vertex.GetPointIds().SetId(0, i);
            //    grid.InsertNextCell((int)VTKCellType.VTK_VERTEX, vertex.GetPointIds());
                
            //   // m_cells .InsertNextCell(vertex);
            //}
            vtkVertexGlyphFilter filter = new vtkVertexGlyphFilter();
            this.GetProperty().SetPointSize(5);
        }
        #region FEMActor成员函数 ,对数据进行响应
        public override void ClearAllSelected()
        {
            m_selectedPointsIds = new Dictionary<int, bool>();
            m_selectedCellsIds = new Dictionary<int, bool>();
            SetDefaultColor();
        }

        public override void AddPointId(int id, ePickBollean boolean)
        {
            Debug.Assert(m_selectedPointsIds != null, "请先初始化m_selectedIds数组");
            bool hasPoint = m_selectedPointsIds.ContainsKey(id);
            if (boolean == ePickBollean.Add && !hasPoint)
                m_selectedPointsIds.Add(id, true);//添加选择的点
            else if (boolean == ePickBollean.Sub && hasPoint)
                m_selectedPointsIds.Remove(id);//删除选择的点
        }

        public override void AddCellId(int id, ePickBollean boolean)
        {
            Debug.Assert(m_selectedCellsIds != null, "请先初始化m_selectedCellsIds数组");
            bool hasCell = m_selectedCellsIds.ContainsKey(id);
            if (boolean == ePickBollean.Add && !hasCell)
                m_selectedCellsIds.Add(id, true);//
            else if (boolean == ePickBollean.Sub && hasCell)
                m_selectedCellsIds.Remove(id);//
            Debug.WriteLine(id.ToString());
        }

        public override void AddPointId(Kitware.VTK.vtkIdTypeArray ids, ePickBollean boolean)
        {
            Debug.Assert(m_selectedPointsIds != null, "请先初始化m_selectedIds数组");
            if (boolean == ePickBollean.Add)
            {
                for (int i=0;i<ids.GetNumberOfTuples();i++)
                {
                    int intid = ids.GetValue(i);
                    bool hasPoint = m_selectedPointsIds.ContainsKey(intid);
                    if (!hasPoint)
                        m_selectedPointsIds.Add(intid, true);
                }
            }
            else if (boolean == ePickBollean.Sub)
            {
                for (int i = 0; i < ids.GetNumberOfTuples(); i++)
                {
                    int intid = ids.GetValue(i);
                    bool hasPoint = m_selectedPointsIds.ContainsKey(intid);
                    if (hasPoint)
                        m_selectedPointsIds.Remove(intid);
                }
            }
        }

        public override void AddCellId(Kitware.VTK.vtkIdTypeArray ids, ePickBollean boolean)
        {
            Debug.Assert(m_selectedCellsIds != null, "请先初始化m_selectedCellsIds数组");
            Debug.WriteLine(ids.GetNumberOfTuples());
            if (boolean == ePickBollean.Add)
            {
                for (int i = 0; i < ids.GetNumberOfTuples(); i++)
                {
                    int intid = ids.GetValue(i);
                    bool hasPoint = m_selectedCellsIds.ContainsKey(intid);
                    if (!hasPoint)
                        m_selectedCellsIds.Add(intid, true);
                }
            }
            else if (boolean == ePickBollean.Sub)
            {
                for (int i = 0; i < ids.GetNumberOfTuples(); i++)
                {
                    int intid = ids.GetValue(i);
                    bool hasPoint = m_selectedCellsIds.ContainsKey(intid);
                    if (hasPoint)
                        m_selectedCellsIds.Remove(intid);
                }
            }
        }
        /// <summary>
        /// 当有点和cell被选择是更新颜色 
        /// </summary>
        public override void UpdateSelectedColor()
        {
            ///cell
            if (m_selectedCellsIds != null && m_selectedCellsIds.Count > 0)
            {
                vtkUnsignedCharArray colors = new vtkUnsignedCharArray();
                colors.SetNumberOfComponents(3);
                int cellNum = this.GetMapper().GetInput().GetNumberOfCells();
                colors.SetNumberOfTuples(cellNum);
                for (int i = 0; i < cellNum; ++i)
                {
                    colors.SetTuple3(i, m_defaultColor [0],m_defaultColor [1],m_defaultColor [2]);
                }
                foreach (int  cellID in m_selectedCellsIds.Keys)
                {
                    colors .SetTuple3(cellID ,ModelUtils.SelectedColor[0], ModelUtils.SelectedColor[1],
                    ModelUtils.SelectedColor[2]);
                }
                this.GetMapper().GetInput().GetCellData().SetScalars(colors);
            }
                ///point
            else if (m_selectedPointsIds != null && m_selectedPointsIds.Count > 0)
            {
                vtkUnsignedCharArray ptcolors = new vtkUnsignedCharArray();
                ptcolors.SetNumberOfComponents(3);
                int ptNum = this.GetMapper().GetInput().GetNumberOfPoints();
                ptcolors.SetNumberOfTuples(ptNum);

                for (int i = 0; i < ptNum; ++i)
                {
                    ptcolors.SetTuple3(i, m_defaultColor[0], m_defaultColor[1], m_defaultColor[2]);
                }
                foreach (int ptid in m_selectedPointsIds.Keys)
                {
                    ptcolors.SetTuple3(ptid, ModelUtils.SelectedColor[0], ModelUtils.SelectedColor[1],ModelUtils.SelectedColor[2]);
                }

                this.GetMapper().SetScalarModeToUsePointData();
                this.GetMapper().GetInput().GetPointData().SetScalars(ptcolors);

                //this.GetProperty().SetPointSize(4);
                //this.GetProperty().SetRepresentationToPoints();

            }
            this.GetMapper().Update();
        }
        public override bool IsSelected
        {
            get;
            set;
        }
        #endregion






    }
}
