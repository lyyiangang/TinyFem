using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kitware.mummy;
using Kitware.VTK;
using TinyFem.Modeling;
namespace TinyFem.VTKForm
{
    /// <summary>
    ///  选择的对象，actor（part）单元，还是节点
    /// </summary>
    public enum ePickTarget
    {
        None, Actor, Cell, Point,
    };
    public enum ePickMode
    {
        None,
        /// <summary>
        /// 点选拾取模式
        /// </summary>
        DotPickMode,
        /// <summary>
        /// 框选拾取模式
        /// </summary>
        RectPickMode
    };

    /// <summary>
    ///  向已选对象列表中添加/删除刚选择的对象
    /// </summary>
    public enum ePickBollean
    {
        None,Add,Sub,
    };
    public class MyInteractorStyle : vtkInteractorStyleRubberBand3D
    {
         vtkRenderWindowInteractor m_iren;
         vtkRenderWindow m_rendWin;
         vtkRenderer m_renderer;
         RenderWindowControl m_RWCtrl;
         ePickTarget m_pickTarget = ePickTarget.None;//默认选择对象为空,选择part/单元/节点
         ePickMode m_pickMode = ePickMode.None;//默认的选择方式：点选/框选
         ePickBollean m_pickBoolean = ePickBollean.None;//选择对象的布尔操作 

         public delegate void ObjectsSeclectedHandler(vtkObject sender, vtkObjectEventArgs e);//有对象被选中
         public event ObjectsSeclectedHandler OnObjectsSelected;

         int endx = 0, endy = 0, startx = 0, starty = 0;// 记录框选的起始点和结束点

         int m_targetNum = 0;
         public MyInteractorStyle(RenderWindowControl  ctrl)
         {
             m_RWCtrl = ctrl;
             m_rendWin = ctrl.RenderWindow ;
             m_renderer = m_rendWin.GetRenderers().GetFirstRenderer();
             m_iren = m_rendWin.GetInteractor();
             
             m_iren .LeftButtonReleaseEvt+=new vtkObjectEventHandler(m_interactor_LeftButtonReleaseEvt);
             
             m_iren.LeftButtonPressEvt += new vtkObjectEventHandler(m_iren_RightButtonPressEvt);
         }
         public void UnHookEventHandler()
         {
             m_iren.LeftButtonReleaseEvt -= m_interactor_LeftButtonReleaseEvt;
             m_iren.LeftButtonPressEvt -= m_iren_RightButtonPressEvt;

         }
         void m_iren_RightButtonPressEvt(vtkObject sender, vtkObjectEventArgs e)
         {//左键按下记录矩形的起始点
             if (m_pickTarget == ePickTarget.None )
                 return;
             if (m_pickMode != ePickMode.RectPickMode)//如果不是框选则不用记录矩形起始点坐标
                 return;
             m_iren.GetEventPosition(ref startx, ref starty );//获取起始点
             endx = startx; endy = starty;
             //Console.WriteLine("startx:{0},starty:{1}", startx, starty);
         }
        /// <summary>
        /// 已选对象的数目
        /// </summary>
         public int TargetNum
         {
             get { return m_targetNum; }
         }
        /// <summary>
        /// 选择方式：点选/框选
        /// </summary>
         public ePickMode PickMode
         {
             get { return m_pickMode; }
             set { m_pickMode = value; }
         }
        /// <summary>
        /// 选择对象：Actor/Cell/Point
        /// </summary>
         public ePickTarget PickTarget
         {
             get { return m_pickTarget; }
             set { m_pickTarget = value; }
         }
        /// <summary>
        /// 选择的布尔操作
        /// </summary>
         public ePickBollean PickBollean
         {
             get { return m_pickBoolean; }
             set { m_pickBoolean = value; }
         }
        /// <summary>
        /// 左键释放时获取选择的对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         void m_interactor_LeftButtonReleaseEvt(vtkObject sender, vtkObjectEventArgs e)
         {
             if (m_pickTarget == ePickTarget.None)
                 return;

             //if (m_iren.GetControlKey() != 0)
             //    m_pickBoolean = ePickBollean.Add;
             //else if (m_iren.GetShiftKey() != 0)
             //    m_pickBoolean = ePickBollean.Sub;
             
             m_iren.GetEventPosition(ref endx, ref endy);
             #region 点选对象
             if (m_pickMode == ePickMode.DotPickMode)//如果是点选模式
             {
                 vtkCellPicker cellPicker = new vtkCellPicker();
                 cellPicker.SetTolerance(0.005);
                 if(0==cellPicker .Pick(endx,endy ,0,m_renderer ))
                     return ;//什么都没有选到就返回

                 FEMActor actor = cellPicker.GetActor() as FEMActor;
                 if (actor == null)
                     return;
                 if (m_pickTarget == ePickTarget.Actor)
                 {//如果选择对象为
                         actor.IsSelected = true;
                         m_RWCtrl.Invalidate();//貌似必须要用这个函数,update 不行
                         OnObjectsSelected(sender, e);
                 }
                else  if (m_pickTarget == ePickTarget.Cell)
                 {
                     int cellId = cellPicker.GetCellId();
                     actor.AddCellId(cellId, m_pickBoolean);
                     OnObjectsSelected(sender, e);
                     //if (cellId > 0)
                     //    Console.WriteLine("dotselect cellid:{0}", cellId);
                 }
                 else if (m_pickTarget == ePickTarget.Point)
                 {
                     int pointId = cellPicker.GetPointId();
                     if (pointId!=-1)
                     {
                         actor.AddPointId(pointId, m_pickBoolean);
                         OnObjectsSelected(sender, e);
                      //   Console.WriteLine("dotselect pointid:{0}", pointId);
                     }
                 }
             }
             #endregion 
             else if (m_pickMode == ePickMode.RectPickMode)//如果是框选
             {
                 vtkAreaPicker areaPicker = new vtkAreaPicker();
                 if (0 == areaPicker.AreaPick(startx ,starty ,endx , endy ,m_renderer ))
                 {//如果什么也没选到就返回
                     //this.impactViewForm.ClearAllSelectCells();
                     return;
                 }
                 vtkProp3DCollection selectedActors = areaPicker.GetProp3Ds();
                 selectedActors.InitTraversal();
                 FEMActor selectedActor;
                 while ((selectedActor = selectedActors.GetNextProp3D() as FEMActor) != null)
                 {
                     vtkSelection selection = new vtkSelection();
                     vtkSelectionNode selectionNode = new vtkSelectionNode();
                     selection.AddNode(selectionNode);

                     vtkIdFilter idFilter = new vtkIdFilter();
                     idFilter.FieldDataOn();
                     idFilter.SetInput(selectedActor.GetMapper().GetInput());
                     idFilter.Update();

                     vtkExtractSelection frustum = new vtkExtractSelection();
                     frustum.SetInput(0, idFilter.GetOutput());
                     frustum.SetInput(1, selection);

                     selectionNode.Initialize();
                     selectionNode.SetContentType((int)vtkSelectionNode.SelectionContent.FRUSTUM);

                     if(m_pickTarget==ePickTarget.Cell)//如果选择的目标是cell
                     selectionNode.SetFieldType((int)vtkSelectionNode.SelectionField.CELL);
                     else if (m_pickTarget ==ePickTarget .Point)//如果选择的目标是point
                     selectionNode.SetFieldType((int)vtkSelectionNode.SelectionField.POINT);

                     vtkPoints points = areaPicker.GetClipPoints();
                     vtkDoubleArray frustumcorners = new vtkDoubleArray();
                     frustumcorners.SetNumberOfComponents(4);
                     frustumcorners.SetNumberOfTuples(8);
                     for (int i = 0; i < 8; ++i)
                     {
                         double[] point = points.GetPoint(i);
                         frustumcorners.SetTuple4(i, point[0], point[1], point[2], 0.0);
                     }
                     selectionNode.SetSelectionList(frustumcorners);

                     frustum.Update();
                     vtkUnstructuredGrid grid = new vtkUnstructuredGrid();
                     grid = vtkUnstructuredGrid.SafeDownCast(frustum.GetOutput());
                     vtkIdTypeArray ids=null ;
                     if (m_pickTarget == ePickTarget.Cell)
                     {
                         ids = vtkIdTypeArray.SafeDownCast(grid.GetCellData().GetArray("vtkOriginalCellIds"));
                         if (ids == null)
                             continue;
                         selectedActor.AddCellId(ids, m_pickBoolean);
                         OnObjectsSelected(sender, e);
                     }
                     else if (m_pickTarget == ePickTarget.Point)
                     {
                         ids = vtkIdTypeArray.SafeDownCast(grid.GetPointData().GetArray("vtkOriginalPointIds"));
                         if (ids == null)
                             continue;
                         selectedActor.AddPointId(ids, m_pickBoolean);
                         OnObjectsSelected(sender, e);
                     }

                 }

             }
         }

    }
}
