using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TinyFem.Base;
namespace TinyFem
{
    public partial class frmMain : DevComponents.DotNetBar.Office2007RibbonForm
    {
        DocumentForm m_activeDocument = null;
        ViewForm m_viewForm = null;
        public frmMain()
        {
            InitializeComponent();
            InitForm();
        }
        void InitForm()
        {//设置控件的基本属性
            this.IsMdiContainer = true;//可以盛放子窗体
            this.ribbonControl1.MdiSystemItemVisible = false;//在窗口右上角不显示最大化最小化按钮
            this.tabStrip1.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabStrip1.MdiTabbedDocuments = true;//在tab上显示多个窗口
            this.tabStrip1.CloseButtonVisible = false;
            m_viewForm = new ViewForm();
            m_viewForm.MdiParent = this;
            m_viewForm.Show();

            m_testbullet.Click += new EventHandler(m_testbullet_Click);
        }
        protected override void OnMdiChildActivate(EventArgs e)
        {
            DocumentForm olddocument = m_activeDocument;
            base.OnMdiChildActivate(e);
            m_activeDocument = this.ActiveMdiChild as DocumentForm;
        }
        void m_testbullet_Click(object sender, EventArgs e)
        {
            m_viewForm.TestShowActor();
        }

        private void m_rbbtOpen_Click(object sender, EventArgs e)
        {
            m_viewForm.TestSelect();
        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void m_testbullet_Click_1(object sender, EventArgs e)
        {

        }


    }
 





}
