using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.D2DLib;
using unvell.D2DLib.WinForm;

namespace d2dlib_winform
{
    public partial class Form1 : D2DForm {
        private D2DPoint _d2DPoint = new D2DPoint(0, 0);
        private D2DSize _d2DSize = new D2DSize(10, 10);
        
        public Form1()
        {
            InitializeComponent();
            base.AnimationDraw = true;
            base.ShowFPS = true;
        }

        protected override void OnFrame() {
            base.OnFrame();
            
            _d2DPoint.x += 1;
            _d2DPoint.y += 1;
        }

        protected override void OnRender(D2DGraphics g) {
            base.OnRender(g);
            var rect = new D2DRect(_d2DPoint, _d2DSize);
            g.DrawRectangle(rect, D2DColor.Red);
        }
    }
}
