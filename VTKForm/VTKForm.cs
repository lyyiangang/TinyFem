using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Kitware.mummy;
using Kitware.VTK;
using TinyFem.Base;
using TinyFem.Modeling;

namespace TinyFem.VTKForm
{
    public partial class VTKFormBase : Form
    {
        public enum eCameraPos
        {
            XTop,YTop,ZTop,XYZ,
        }
        public enum eInteractorStyle
        {
            SelectStyle,DefaultStyle,
        }
        protected  vtkRenderer m_render=null;
        protected vtkRenderWindow m_renWindow = null;
        protected vtkRenderWindowInteractor m_iren = null;
        protected Dictionary<FEMActor, bool> m_actorDict = new Dictionary<FEMActor, bool>();
        vtkOrientationMarkerWidget m_marker=null;// 坐标轴
        vtkInteractorStyle m_currentInteractorStyle=null ;
        eInteractorStyle m_interactorStyle = eInteractorStyle.DefaultStyle;
        public VTKFormBase()
        {
            InitializeComponent();
            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Fill;
        }
        public eInteractorStyle InteractorStyle
        {
            get
            { 
                return m_interactorStyle;
            }
            set 
            {
                if (value == eInteractorStyle.SelectStyle)
                {
                    MyInteractorStyle mystle = new MyInteractorStyle(m_renderWinCtrl);
                    mystle.PickMode = ePickMode.RectPickMode;
                    mystle.PickTarget = ePickTarget.Point;
                    m_iren.SetInteractorStyle(mystle);
                    mystle.OnObjectsSelected += new MyInteractorStyle.ObjectsSeclectedHandler(mystle_OnObjectsSelected);
                    m_currentInteractorStyle = mystle;
                }
                else if (value == eInteractorStyle.DefaultStyle)
                {
                    var myinteractorsytle = m_iren.GetInteractorStyle() as MyInteractorStyle;
                    if (myinteractorsytle != null)
                    {
                        myinteractorsytle.UnHookEventHandler();//取消鼠标响应事件
                        myinteractorsytle.OnObjectsSelected -= mystle_OnObjectsSelected;//当有对象别选中时
                    }
                    vtkInteractorStyleTrackballCamera style = new vtkInteractorStyleTrackballCamera();
                    m_iren.SetInteractorStyle(style);
                    m_currentInteractorStyle = style;//当前sytle
                }
                InitSelectedObject();
                m_interactorStyle = value;
            }
        }
        /// <summary>
        /// 被选中对象高光
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mystle_OnObjectsSelected(vtkObject sender, vtkObjectEventArgs e)
        {
            foreach (var item in m_actorDict.Keys)
                item.UpdateSelectedColor();
            m_renderWinCtrl.Invalidate();
        }
        /// <summary>
        /// 在进行选择初始化被选择的对象将已经选择的point，cell都清零
        /// </summary>
        void InitSelectedObject()
        {
            foreach (var item in m_actorDict.Keys)
                item.ClearAllSelected();
            m_renderWinCtrl.Invalidate();
        }
        public vtkInteractorStyle CurrentInteractorStyle
        {
            get { return m_currentInteractorStyle; }
            set { m_currentInteractorStyle = value; }
        }
        public void AddActor(FEMActor  actor)
        {
            if(m_actorDict .ContainsKey(actor ))
                return ;
            m_actorDict .Add (actor ,true );
            m_render.AddActor(actor);
            SetViewPort(eCameraPos.YTop);//设置视角
            m_renderWinCtrl.Invalidate();
        }
        public void RemoveActor(FEMActor  actor)
        {
            if (!m_actorDict.ContainsKey(actor))
                return;
             m_actorDict.Remove(actor);
             m_render.RemoveActor(actor);
             SetViewPort(eCameraPos.YTop);//设置视角
             m_renderWinCtrl.Invalidate();//重绘
        }


        public void SetActorVisiable(FEMActor  actor, bool isVisiable)
        {
            if (!m_actorDict.ContainsKey(actor))
                return;
            if (isVisiable)
                actor.SetVisibility(1);
            else
                actor.SetVisibility(0);
        }
        private void m_renderWinCtrl_Load(object sender, EventArgs e)
        {
            m_renWindow = m_renderWinCtrl.RenderWindow;
            m_render = m_renWindow.GetRenderers().GetFirstRenderer();
            m_iren = m_renWindow.GetInteractor();

            InitRenderWindow();
        }

        void InitRenderWindow()
        {
            m_render.GradientBackgroundOn();
            m_render.SetBackground(32.0 / 255, 82.0 / 255, 112.0 / 255);
            m_render.SetBackground2(198.0 / 255, 198.0 / 255, 219.0 / 255);
            DrawAxes();
        }
        void tsBt_Click(object sender, EventArgs e)
        {
            ToolStripButton bt = sender as ToolStripButton;
            if (bt == null)
                return;
            string tag = bt.Tag as string;
            if (tag == null||tag==string.Empty )
                return;
            switch (tag)
            {//设置视角 
                case "viewX": SetViewPort(eCameraPos.XTop); break;
                case "viewY": SetViewPort(eCameraPos.YTop); break;
                case "viewZ": SetViewPort(eCameraPos.ZTop); break;
                case "viewXYZ": SetViewPort(eCameraPos.XYZ); break;
                case "select": ; break;
                case "reset": Test(); break;
                default: break;
            }
        }

        void Test()
        {
            foreach (var actor in m_actorDict.Keys  )
            {
                int xmin = 0;
                int xlength = 1000;
                int xmax = xmin + xlength;
                int ymin = 0;
                int ylength = 1000;
                int ymax = ymin + ylength;

                int[] pos = { xmin, xmin+xlength ,ymin ,ymin +ylength };
                #region RECT
                vtkPoints pts = vtkPoints.New();
                pts.InsertPoint(0, xmin, ymin, 0);
                pts.InsertPoint(1, xmax, ymin, 0);
                pts.InsertPoint(2, xmax, ymax, 0);
                pts.InsertPoint(3, xmin, ymax, 0);
                vtkCellArray rect = vtkCellArray.New();
                rect.InsertNextCell(5);
                rect.InsertCellPoint(0);
                rect.InsertCellPoint(1);
                rect.InsertCellPoint(2);
                rect.InsertCellPoint(3);
                rect.InsertCellPoint(0);
                vtkPolyData selectRect = vtkPolyData.New();
                selectRect.SetPoints(pts);
                selectRect.SetLines(rect);
                vtkPolyDataMapper2D rectMapper = vtkPolyDataMapper2D.New();
                rectMapper.SetInput(selectRect);
                vtkActor2D rectActor = vtkActor2D.New();
                rectActor.SetMapper(rectMapper);
                m_render.AddActor(rectActor);
                #endregion
                vtkIdFilter ids = vtkIdFilter.New();
                ids.SetInput(actor.GetMapper().GetInput());
               //ids.SetInputConnection( actor.GetMapper().GetOutputPort());
                ids.PointIdsOn();
                ids.FieldDataOn();

                vtkSelectVisiblePoints visPts = vtkSelectVisiblePoints.New();
                visPts.SetInput(ids.GetOutput());
                visPts.SetRenderer(m_render);
                visPts.SelectInvisibleOn();
                visPts.SelectionWindowOn();
                //visPts.SelectInvisibleOff();
                visPts.SetSelection(pos[0], pos[1], pos[2], pos[3]);

                vtkLabeledDataMapper labelMapper = vtkLabeledDataMapper.New();
                labelMapper.SetInputConnection(visPts.GetOutputPort());
               // labelMapper.SetInput(visPts.GetInput());
                labelMapper.SetLabelModeToLabelFieldData();
                 vtkActor2D actor2d = vtkActor2D.New();
                 actor2d.SetMapper(labelMapper);
                 m_render.AddActor(actor2d);
            }
            m_render.Render();
        }
        void Test2()
        {
            int xmin = 0;
            int xlength = 1000;
            int xmax = xmin + xlength;
            int ymin = 0;
            int ylength = 1000;
            int ymax = ymin + ylength;

            #region 定义显示的rectActor
            vtkPoints pts = vtkPoints.New();
            pts.InsertPoint(0, xmin, ymin, 0);
            pts.InsertPoint(1, xmax, ymin, 0);
            pts.InsertPoint(2, xmax, ymax, 0);
            pts.InsertPoint(3, xmin, ymax, 0);
            vtkCellArray rect = vtkCellArray.New();
             rect.InsertNextCell(5);
             rect.InsertCellPoint(0);
             rect.InsertCellPoint(1);
             rect.InsertCellPoint(2);
             rect.InsertCellPoint(3);
             rect.InsertCellPoint(0);
             vtkPolyData selectRect = vtkPolyData.New ();
             selectRect.SetPoints(pts);
             selectRect.SetLines(rect);
             vtkPolyDataMapper2D rectMapper = vtkPolyDataMapper2D.New ();
             rectMapper.SetInput(selectRect);
             vtkActor2D rectActor = vtkActor2D.New() ;
             rectActor.SetMapper(rectMapper);
            #endregion

             vtkSphereSource sphere = vtkSphereSource.New ();
             vtkPolyDataMapper sphereMapper = vtkPolyDataMapper.New ();
             sphereMapper.SetInputConnection(sphere.GetOutputPort());
            // sphereMapper.SetImmediateModeRendering(1);
             vtkActor sphereActor =vtkActor.New ();
             sphereActor.SetMapper(sphereMapper);

             vtkIdFilter ids =vtkIdFilter.New ();
             ids.SetInputConnection(sphere.GetOutputPort());
             ids.PointIdsOn();
             ids.CellIdsOn();
             ids.FieldDataOn();

             #region 设置要显示的点的及其label
             vtkSelectVisiblePoints visPts = vtkSelectVisiblePoints.New ();
             visPts.SetInputConnection(ids.GetOutputPort());
             visPts.SetRenderer(m_render);
             visPts.SelectionWindowOn();
             visPts.SetSelection(xmin, xmin + xlength, ymin, ymin + ylength);
 
             vtkLabeledDataMapper pointsMapper = vtkLabeledDataMapper.New ();
             pointsMapper.SetInputConnection(visPts.GetOutputPort());
             pointsMapper.SetLabelModeToLabelFieldData();
             pointsMapper.GetLabelTextProperty().SetColor(0, 255, 0);
             pointsMapper.GetLabelTextProperty().BoldOff();
             vtkActor2D pointLabels = vtkActor2D.New();
             pointLabels.SetMapper(pointsMapper);
             #endregion 

             #region 设置要显示的cell的id及其label
             vtkCellCenters cc = vtkCellCenters.New ();
             cc.SetInputConnection(ids.GetOutputPort());
             vtkSelectVisiblePoints visCells = vtkSelectVisiblePoints.New ();
             visCells.SetInputConnection(cc.GetOutputPort());
             visCells.SetRenderer(m_render );
             visCells.SelectionWindowOn();
             visCells.SetSelection(xmin, xmin + xlength, ymin, ymin + ylength);
 
            ///显示每个Cell的id
             vtkLabeledDataMapper cellMapper = vtkLabeledDataMapper.New ();
             cellMapper.SetInputConnection(visCells.GetOutputPort());
             cellMapper.SetLabelModeToLabelFieldData();
             cellMapper.GetLabelTextProperty().SetColor(255, 0, 0);
             vtkActor2D cellLabels = vtkActor2D.New();
             cellLabels.SetMapper(cellMapper);
             #endregion

             m_render.AddActor(sphereActor);
             m_render.AddActor2D(rectActor);
             m_render.AddActor2D(pointLabels);
            // m_render.AddActor2D(cellLabels);
        }
        void DrawAxes()
        {// 设置坐标轴
            if (null == m_marker)
            {
                vtkAxesActor axes = new vtkAxesActor();
                axes.SetShaftTypeToLine();
                axes.SetXAxisLabelText("x");
                axes.SetYAxisLabelText("y");
                axes.SetZAxisLabelText("z");
                axes.SetTotalLength(0.6, 0.6, 0.6);
                m_marker = new vtkOrientationMarkerWidget();

                m_marker.SetInteractor(m_iren);
                m_marker.SetOrientationMarker(axes);
                m_marker.SetViewport(-0.2, -0.2, 0.3, 0.3);
                m_marker.SetEnabled(1);
                m_marker.SetInteractive(0);
            }
        }
        public void SetViewPort(eCameraPos camerapos)
        {
            vtkCamera cam = m_render.GetActiveCamera();
            cam.SetFocalPoint(0, 0, 0);
            switch (camerapos)
            {
                case eCameraPos.XTop:
                    {
                        cam.SetPosition(0, 0, -1); 
                        cam.ComputeViewPlaneNormal();
                        cam.SetViewUp(0, 1, 0);
                    } break;
                case eCameraPos.YTop: 
                    {
                        cam.SetPosition(0, -1, 0);
                        cam.ComputeViewPlaneNormal();
                        cam.SetViewUp(0, 0, 1);

                    } break;
                case eCameraPos.ZTop: 
                    {
                        cam.SetPosition(-1, 0, 0);
                        cam.ComputeViewPlaneNormal();
                        cam.SetViewUp(0, 1, 0);
                    } break;
                case eCameraPos.XYZ:
                    {
                        cam.SetPosition(1, 1, 1);
                        cam.ComputeViewPlaneNormal();
                        cam.SetViewUp(0, 1, 0);
                    }
                    break;
                default: 
                    {
                        cam.SetPosition(0, 0, 0);
                        cam.ComputeViewPlaneNormal();
                        cam.SetViewUp(0, 1, 0);
                    } break;
            }

            m_render.ResetCamera();
            m_renderWinCtrl.Refresh();
        }
        /// <summary>
        /// 添加自定义交互方式
        /// </summary>

    }
}
