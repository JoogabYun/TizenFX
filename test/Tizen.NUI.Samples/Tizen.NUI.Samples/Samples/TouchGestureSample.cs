using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Events;


namespace Tizen.NUI.Samples
{
    public class TouchGestureSample : IExample
    {
        private View root;
        private View frontView;
        private TextLabel backView;
        private TapGestureDetector tapGestureDetector;
        private LongPressGestureDetector longPressGestureDetector;
        private LongPressGestureDetector longPressGestureDetector2;
        private GestureDetector detector;

        public void Activate()
        {
            // NUIApplication.SetGeometryHittestEnabled(true);
            Window window = NUIApplication.GetDefaultWindow();

            frontView = new View
            {
                Size = new Size(300, 300),
                Position = new Position(150, 170),
                BackgroundColor = new Color(1.0f, 0.0f, 0.0f, 1.0f),
                ClippingMode = ClippingModeType.ClipToBoundingBox,
            };
            // frontView.TouchEvent += OnFrontTouchEvent;
            window.Add(frontView);

            tapGestureDetector = new TapGestureDetector();
            tapGestureDetector.Detected += (s, e) =>
            {
                Tizen.Log.Error("NUI", $"tapGestureDetector\n");
            };

            longPressGestureDetector = new LongPressGestureDetector();
            longPressGestureDetector.Detected += (s, e) =>
            {
                Tizen.Log.Error("NUI", $"longPressGestureDetector==========================================\n");
            };


            var otherView = new View
            {
                Size2D = new Size2D(300, 300),
                Position = new Position(100, 100),
                BackgroundColor = Color.Blue,
                // DrawMode = DrawModeType.Overlay2D,
            };
            otherView.TouchEvent += (s, e) =>
            {
                Tizen.Log.Error("NUI", $"otherView touch {e.Touch.GetState(0)}\n");
                return true;
            };
            longPressGestureDetector2 = new LongPressGestureDetector();
            longPressGestureDetector2.Attach(frontView);
            longPressGestureDetector2.Detected += (s, e) =>
            {
                Tizen.Log.Error("NUI", $"longPressGestureDetector2==========================================\n");
            };
            frontView.Add(otherView);


        }

        private bool OnFrontTouchEvent(object source, View.TouchEventArgs e)
        {
            Tizen.Log.Error("NUI", $"OnFrontTouchEvent {e.Touch.GetState(0)}\n");
            return longPressGestureDetector.FeedTouch(source as View, e.Touch);
        }


        private bool OnBackTouchEvent(object source, View.TouchEventArgs e)
        {
            Tizen.Log.Error("NUI", $"OnBackTouchEvent {e.Touch.GetState(0)}\n");
            return false;
        }

        public void Deactivate()
        {
            if (root != null)
            {
                NUIApplication.GetDefaultWindow().Remove(root);
                root.Dispose();
            }
        }
    }
}
