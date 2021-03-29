using unvell.D2DLib;
using unvell.D2DLib.WinForm;

namespace d2dlib_winform {
    public partial class Form1 : D2DForm {
        private D2DPoint _d2DPoint = new(0, 0);
        private readonly D2DSize _d2DSize = new(10, 10);

        public Form1() {
            InitializeComponent();
            AnimationDraw = true;
            ShowFPS = true;
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