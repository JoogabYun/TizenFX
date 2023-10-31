using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Events;


namespace Tizen.NUI.Samples
{
    public class TouchGestureSample : IExample
    {
        private View root;
        private View redView;
        private View blueView;
        private TextLabel backView;

        private PanGestureDetector panGestureDetector;
        private PanGestureDetector panGestureDetector1;


        private LongPressGestureDetector longPressGestureDetector;
        private LongPressGestureDetector longPressGestureDetector2;

        private TapGestureDetector tapGestureDetector;
        private TapGestureDetector tapGestureDetector2;

        private GestureDetector detector;

        public void Activate()
        {
            NUIApplication.SetGeometryHittestEnabled(true);
            Window window = NUIApplication.GetDefaultWindow();

            redView = new View
            {
                Size = new Size(300, 300),
                Position = new Position(150, 170),
                BackgroundColor = new Color(1.0f, 0.0f, 0.0f, 1.0f),
                ClippingMode = ClippingModeType.ClipToBoundingBox,
            };
            window.Add(redView);

            blueView = new View
            {
                Size2D = new Size2D(300, 300),
                Position = new Position(100, 100),
                // Position = new Position(150, 170),
                BackgroundColor = Color.Blue,
            };

            var textField = new TextField()
            {
                Text = "Input",
                Size2D = new Size2D(300, 100),
                Position = new Position(100, 100),
                BackgroundColor = Color.Orange
            };
            // blueView.Add(textField);
            textField.TouchEvent += (s, e) =>
            {
                return false;
            };

            // redView.Add(blueView);
            window.Add(blueView);
            window.Add(textField);

            // Pan();
            Tap();
            // LongPress();
        }

        private void Tap()
        {
            tapGestureDetector = new TapGestureDetector();
            // tapGestureDetector.Attach(blueView);
            tapGestureDetector.Detected += (s, e) =>
            {
                 if(e.View == redView)
                {
                    Tizen.Log.Error("NUI", $"redView \n");
                }
                if(e.View == blueView)
                {
                    Tizen.Log.Error("NUI", $"blueView \n");
                }
                if(e.TapGesture.State == Gesture.StateType.Started)
                {
                    e.View.BackgroundColor = Color.Yellow;
                }
                else if (e.TapGesture.State == Gesture.StateType.Finished || e.TapGesture.State == Gesture.StateType.Cancelled)
                {
                    if(e.View == redView)
                    {
                        e.View.BackgroundColor = Color.Red;
                    }
                    if(e.View == blueView)
                    {
                        e.View.BackgroundColor = Color.Blue;
                    }
                }
                Tizen.Log.Error("NUI", $"tapGestureDetector {e.TapGesture.State}\n");
            };

            tapGestureDetector2 = new TapGestureDetector();
            tapGestureDetector2.Detected += (s, e) =>
            {
                 if(e.View == redView)
                {
                    Tizen.Log.Error("NUI", $"redView \n");
                }
                if(e.View == blueView)
                {
                    Tizen.Log.Error("NUI", $"blueView \n");
                }
                if(e.TapGesture.State == Gesture.StateType.Started)
                {
                    e.View.BackgroundColor = Color.Yellow;
                }
                else if (e.TapGesture.State == Gesture.StateType.Finished || e.TapGesture.State == Gesture.StateType.Cancelled)
                {
                    if(e.View == redView)
                    {
                        e.View.BackgroundColor = Color.Red;
                    }
                    if(e.View == blueView)
                    {
                        e.View.BackgroundColor = Color.Blue;
                    }
                }
                Tizen.Log.Error("NUI", $"tapGestureDetector2 {e.TapGesture.State}\n");
            };

            redView.TouchEvent += (s, e) =>
            {
                 Tizen.Log.Error("NUI", $"Tap redView Touch!!!!!!!! {e.Touch.GetState(0)}\n");
                 if(e.Touch.GetState(0) == PointStateType.Down || e.Touch.GetState(0) == PointStateType.Interrupted)
                 {
                    redView.BackgroundColor = Color.Red;
                 }
                return tapGestureDetector.HandleEvent(s as View, e.Touch);
            };

            blueView.TouchEvent += (s, e) =>
            {

                 Tizen.Log.Error("NUI", $"Tap blueView Touch!!!!!!!! {e.Touch.GetState(0)}\n");
                 if(e.Touch.GetState(0) == PointStateType.Down || e.Touch.GetState(0) == PointStateType.Interrupted)
                 {
                    blueView.BackgroundColor = Color.Blue;
                 }
                return tapGestureDetector.HandleEvent(s as View, e.Touch);
            };
        }

        private void Pan()
        {
            panGestureDetector = new PanGestureDetector();
            panGestureDetector.SetMaximumTouchesRequired(2);
            panGestureDetector.Detected += (s, e) =>
            {
                if(e.View == redView)
                {
                    Tizen.Log.Error("NUI", $"redView {e.PanGesture.ScreenDisplacement.X}, {e.PanGesture.ScreenDisplacement.Y} \n");
                }
                if(e.View == blueView)
                {
                    Tizen.Log.Error("NUI", $"blueView {e.PanGesture.ScreenDisplacement.X}, {e.PanGesture.ScreenDisplacement.Y}\n");
                }
                e.View.Position += new Position(e.PanGesture.ScreenDisplacement.X, e.PanGesture.ScreenDisplacement.Y);
                Tizen.Log.Error("NUI", $"panGestureDetector {e.PanGesture.State}\n");
                // e.Handled = false;
            };

            panGestureDetector1 = new PanGestureDetector();
            panGestureDetector1.SetMaximumTouchesRequired(2);
            panGestureDetector1.Detected += (s, e) =>
            {
                e.View.Position += new Position(e.PanGesture.ScreenDisplacement.X, e.PanGesture.ScreenDisplacement.Y);
                Tizen.Log.Error("NUI", $"panGestureDetector1 {e.PanGesture.State}\n");
                if(e.View == redView)
                {
                    Tizen.Log.Error("NUI", $"redView {e.PanGesture.ScreenDisplacement.X}, {e.PanGesture.ScreenDisplacement.Y} \n");
                }
                if(e.View == blueView)
                {
                    Tizen.Log.Error("NUI", $"blueView {e.PanGesture.ScreenDisplacement.X}, {e.PanGesture.ScreenDisplacement.Y}\n");
                }
                // e.Handled = false;
            };

            redView.TouchEvent += (s, e) =>
            {
                Tizen.Log.Error("NUI", $"Pan redView Touch!!!!!!!!\n");
                // return panGestureDetector.HandleEvent(s as View, e.Touch);;
                panGestureDetector.HandleEvent(s as View, e.Touch);;
                return false;
            };

            blueView.TouchEvent += (s, e) =>
            {
                Tizen.Log.Error("NUI", $"Pan blueView Touch!!!!!!!!\n");
                // return panGestureDetector1.HandleEvent(s as View, e.Touch);
                panGestureDetector.HandleEvent(s as View, e.Touch);
                return false;

            };
        }

        private void LongPress()
        {
            longPressGestureDetector = new LongPressGestureDetector();
            // longPressGestureDetector.Attach(redView);
            longPressGestureDetector.Detected += (s, e) =>
            {
                if(e.View == redView)
                {
                    Tizen.Log.Error("NUI", $"redView \n");
                }
                if(e.View == blueView)
                {
                    Tizen.Log.Error("NUI", $"blueView \n");
                }
                if(e.LongPressGesture.State == Gesture.StateType.Started)
                {
                    e.View.BackgroundColor = Color.Yellow;
                }
                else if (e.LongPressGesture.State == Gesture.StateType.Finished || e.LongPressGesture.State == Gesture.StateType.Cancelled)
                {
                    if(e.View == redView)
                    {
                        e.View.BackgroundColor = Color.Red;
                    }
                    if(e.View == blueView)
                    {
                        e.View.BackgroundColor = Color.Blue;
                    }
                }
                Tizen.Log.Error("NUI", $"longPressGestureDetector {e.LongPressGesture.State}\n");
            };

            longPressGestureDetector2 = new LongPressGestureDetector();
            // longPressGestureDetector.Attach(blueView);
            longPressGestureDetector2.Detected += (s, e) =>
            {
                if(e.View == redView)
                {
                    Tizen.Log.Error("NUI", $"redView \n");
                }
                if(e.View == blueView)
                {
                    Tizen.Log.Error("NUI", $"blueView \n");
                }
                if(e.LongPressGesture.State == Gesture.StateType.Started)
                {
                    e.View.BackgroundColor = Color.Yellow;
                }
                else if (e.LongPressGesture.State == Gesture.StateType.Finished || e.LongPressGesture.State == Gesture.StateType.Cancelled)
                {
                    if(e.View == redView)
                    {
                        e.View.BackgroundColor = Color.Red;
                    }
                    if(e.View == blueView)
                    {
                        e.View.BackgroundColor = Color.Blue;
                    }
                }
                Tizen.Log.Error("NUI", $"longPressGestureDetector2 {e.LongPressGesture.State}\n");
            };

            redView.TouchEvent += (s, e) =>
            {
                 Tizen.Log.Error("NUI", $"Long redView Touch!!!!!!!! {e.Touch.GetState(0)}\n");
                return longPressGestureDetector2.HandleEvent(s as View, e.Touch);
            };

            blueView.TouchEvent += (s, e) =>
            {
                Tizen.Log.Error("NUI", $"Long blueView Touch!!!!!!!! {e.Touch.GetState(0)}\n");
                return longPressGestureDetector2.HandleEvent(s as View, e.Touch);
            };
        }

        private bool OnFrontTouchEvent(object source, View.TouchEventArgs e)
        {
            Tizen.Log.Error("NUI", $"OnFrontTouchEvent {e.Touch.GetState(0)}\n");
            return longPressGestureDetector.HandleEvent(source as View, e.Touch);
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
