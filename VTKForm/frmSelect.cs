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
using System.Diagnostics;
namespace TinyFem.VTKForm
{
    public partial class frmSelect :  Base.DocumentForm
    {
        MyInteractorStyle m_style=null ;
        VTKFormBase m_vtkFormbase;
        //public frmSelect()
        //{
        //    InitializeComponent();
        //}
        public frmSelect(VTKFormBase frmbase)
        {
            InitializeComponent();
            m_vtkFormbase = frmbase;
            //m_vtkFormbase.InitSelectedObject();
            m_vtkFormbase.InteractorStyle = VTKFormBase.eInteractorStyle.SelectStyle;//设置成选择模式
            m_style = frmbase.CurrentInteractorStyle as MyInteractorStyle;
            Debug.Assert(m_style != null, "m_style为空");

            m_style.PickTarget = ePickTarget.Point   ;
            m_style.PickMode = ePickMode.DotPickMode;
            m_style.PickBollean = ePickBollean.Add ;

            m_rbtSelect.Click += new EventHandler(m_rbtSelect_Click);//选择反选
            m_rbtUnSelect.Click += new EventHandler(m_rbtSelect_Click);

            m_rbtDotPick.Click += new EventHandler(m_rbtDotPick_Click);//点选，框选
            m_rbtRectPick.Click += new EventHandler(m_rbtDotPick_Click);


        }
        void onClose(object sender, EventArgs e)
        {
        }
        void m_rbtSelect_Click(object sender, EventArgs e)
        {
            if(m_style ==null )
                return ;
            if (m_rbtSelect.Checked)
                m_style.PickBollean = ePickBollean.Add;
            else if (m_rbtUnSelect.Checked)
                m_style.PickBollean = ePickBollean.Sub;

        }
        void m_rbtDotPick_Click(object sender, EventArgs e)
        {
            if (m_style == null)
                return;
            if (m_rbtDotPick.Checked)
                m_style.PickMode = ePickMode.DotPickMode;
            else if (m_rbtRectPick.Checked)
                m_style.PickMode = ePickMode.RectPickMode;
        }

        private void frmSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_vtkFormbase.InteractorStyle = VTKFormBase.eInteractorStyle.DefaultStyle;//设置成默认交互模式
        }
    }
}
