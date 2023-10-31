using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Events;


namespace Tizen.NUI.Samples
{
    public class FeedNestedPanGestureSample_bak : IExample
    {
        private View root;

        public class MyScrollView : View {
            public enum Direction
            {
                Horizontal,
                Vertical
            }
            public View ContentContainer { get; set; }

            private PanGestureDetector mPanGestureDetector;
            private Direction mScrollDirection = Direction.Vertical;
            private float maxScrollDistance;
            private float childTargetPosition = 0.0f;
            private Animation scrollAnimation = new Animation();
            private float velocityOfLastPan = 0.0f;
            private float panAnimationDuration = 0.0f;
            private float panAnimationDelta = 0.0f;
            private float logValueOfDeceleration = 0.0f;
            private float decelerationRate = 0.0f;

            public MyScrollView() : base()
            {
                Tizen.Log.Error("NUI", $"MyScrollView\n");
                ClippingMode = ClippingModeType.ClipToBoundingBox;
                base.Layout = new AbsoluteLayout();
                ContentContainer = new View()
                {
                    BackgroundColor = Color.Grey,
                };

                ContentContainer.Relayout += OnScrollingChildRelayout;
                base.Add(ContentContainer);
                base.Relayout += OnScrollingChildRelayout;
                // this.TouchEvent += OnTouchEvent;
                mPanGestureDetector = new PanGestureDetector();
                mPanGestureDetector.Attach(this);
                mPanGestureDetector.Detected += OnPanGestureDetected;
            }

            public override void Add(View view)
            {
                ContentContainer.Add(view);
            }

            public void SetDirection(Direction direction)
            {
                mScrollDirection = direction;
                mPanGestureDetector.ClearAngles();
                mPanGestureDetector.AddDirection(direction == Direction.Horizontal ?
                        PanGestureDetector.DirectionHorizontal : PanGestureDetector.DirectionVertical);
            }

            private bool OnInterceptTouchEvent(object source, View.TouchEventArgs e)
            {
                Tizen.Log.Error("NUI", $"OnInterceptTouchEvent\n");
                return true;
            }

            private bool OnTouchEvent(object source, View.TouchEventArgs e)
            {
                // bool ret = mPanGestureDetector.HandleEvent(source as View, e.Touch);
                // Tizen.Log.Error("NUI", $"OnTouchEvent {e.Touch.GetState(0)} : {ret}\n");
                return true;
            }

            private void OnPanGestureDetected(object source, PanGestureDetector.DetectedEventArgs e)
            {
                if(e.PanGesture.State == Gesture.StateType.Started)
                {
                    Tizen.Log.Error("NUI", $"OnPanGestureDetected Started\n");
                    e.View.InterceptTouchEvent += OnInterceptTouchEvent;
                }
                else if (e.PanGesture.State == Gesture.StateType.Continuing)
                {
                    Tizen.Log.Error("NUI", $"OnPanGestureDetected mScrollDirection : {mScrollDirection} \n");
                    if(mScrollDirection == Direction.Horizontal)
                    {
                        ScrollBy(e.PanGesture.Displacement.X);
                    }
                    else
                    {
                        ScrollBy(e.PanGesture.Displacement.Y);
                    }

                }
                else if (e.PanGesture.State == Gesture.StateType.Finished || e.PanGesture.State == Gesture.StateType.Cancelled)
                {
                    Tizen.Log.Error("NUI", $"OnPanGestureDetected Finished or Cancelled\n");
                    e.View.InterceptTouchEvent -= OnInterceptTouchEvent;


                    float panVelocity = (mScrollDirection == Direction.Horizontal) ? e.PanGesture.Velocity.X : e.PanGesture.Velocity.Y;
                    if(panVelocity > 0.0f || panVelocity < 0.0f)
                    {
                        Decelerating(panVelocity, scrollAnimation);
                    }

                }
            }

            protected virtual void Decelerating(float velocity, Animation animation)
            {
                if (animation == null) throw new ArgumentNullException(nameof(animation));
                // Decelerating using deceleration equation ===========
                //
                // V   : velocity (pixel per millisecond)
                // V0  : initial velocity
                // d   : deceleration rate,
                // t   : time
                // X   : final position after decelerating
                // log : natural logarithm
                //
                // V(t) = V0 * d pow t;
                // X(t) = V0 * (d pow t - 1) / log d;  <-- Integrate the velocity function
                // X(∞) = V0 * d / (1 - d); <-- Result using infinite T can be final position because T is tending to infinity.
                //
                // Because of final T is tending to infinity, we should use threshold value to finish.
                // Final T = log(-threshold * log d / |V0| ) / log d;

                velocityOfLastPan = Math.Abs(velocity);

                float currentScrollPosition = -(mScrollDirection == Direction.Horizontal ? ContentContainer.CurrentPosition.X : ContentContainer.CurrentPosition.Y);
                panAnimationDelta = (velocityOfLastPan * decelerationRate) / (1 - decelerationRate);
                panAnimationDelta = velocity > 0 ? -panAnimationDelta : panAnimationDelta;

                float destination = -(panAnimationDelta + currentScrollPosition);
                float adjustDestination = destination;
                float maxPosition = maxScrollDistance;
                float minPosition = 0;
                float DecelerationThreshold = 0.1f;

                Tizen.Log.Error("NUI", $"destination {destination} -maxPosition -{maxPosition}, minPosition {minPosition}\n");
                if (destination < -maxPosition || destination > minPosition)
                {
                    panAnimationDelta = velocity > 0 ? (currentScrollPosition - minPosition) : (maxPosition - currentScrollPosition);
                    destination = velocity > 0 ? minPosition : -maxPosition;

                    if (panAnimationDelta == 0)
                    {
                        panAnimationDuration = 0.0f;
                    }
                    else
                    {
                        panAnimationDuration = (float)Math.Log((panAnimationDelta * logValueOfDeceleration / velocityOfLastPan + 1), decelerationRate);
                    }
                    Tizen.Log.Error("NUI", $"panAnimationDuration1 {panAnimationDuration}\n");

                    // Debug.WriteLineIf(LayoutDebugScrollableBase, "\n" +
                    //     "OverRange======================= \n" +
                    //     "[decelerationRate] " + decelerationRate + "\n" +
                    //     "[logValueOfDeceleration] " + logValueOfDeceleration + "\n" +
                    //     "[Velocity] " + velocityOfLastPan + "\n" +
                    //     "[CurrentPosition] " + currentScrollPosition + "\n" +
                    //     "[CandidateDelta] " + panAnimationDelta + "\n" +
                    //     "[Destination] " + destination + "\n" +
                    //     "[Duration] " + panAnimationDuration + "\n" +
                    //     "================================ \n"
                    // );
                }
                else
                {
                    panAnimationDuration = (float)Math.Log(-DecelerationThreshold * logValueOfDeceleration / velocityOfLastPan) / logValueOfDeceleration;

                    Tizen.Log.Error("NUI", $"panAnimationDuration {panAnimationDuration} destination {destination}\n");
                    if (adjustDestination != destination)
                    {
                        destination = adjustDestination;
                        panAnimationDelta = destination + currentScrollPosition;
                        velocityOfLastPan = Math.Abs(panAnimationDelta * logValueOfDeceleration / ((float)Math.Pow(decelerationRate, panAnimationDuration) - 1));
                        panAnimationDuration = (float)Math.Log(-DecelerationThreshold * logValueOfDeceleration / velocityOfLastPan) / logValueOfDeceleration;
                    }

                    // Debug.WriteLineIf(LayoutDebugScrollableBase, "\n" +
                    //     "================================ \n" +
                    //     "[decelerationRate] " + decelerationRate + "\n" +
                    //     "[logValueOfDeceleration] " + logValueOfDeceleration + "\n" +
                    //     "[Velocity] " + velocityOfLastPan + "\n" +
                    //     "[CurrentPosition] " + currentScrollPosition + "\n" +
                    //     "[CandidateDelta] " + panAnimationDelta + "\n" +
                    //     "[Destination] " + destination + "\n" +
                    //     "[Duration] " + panAnimationDuration + "\n" +
                    //     "================================ \n"
                    // );
                }

                // finalTargetPosition = destination;

                // customScrollAlphaFunction = new UserAlphaFunctionDelegate(CustomScrollAlphaFunction);
                // animation.DefaultAlphaFunction = new AlphaFunction(customScrollAlphaFunction);
                // GC.KeepAlive(customScrollAlphaFunction);
                Tizen.Log.Error("NUI", $"panAnimationDuration {panAnimationDuration}\n");
                animation.Duration = (int)panAnimationDuration;
                // animation.Duration = 1000;
                animation.AnimateTo(ContentContainer, (mScrollDirection == Direction.Horizontal) ? "PositionX" : "PositionY", destination);
                animation.Play();
            }

            private void OnScrollingChildRelayout(object source, EventArgs args)
            {
                maxScrollDistance = CalculateMaximumScrollDistance();
            }

            private float CalculateMaximumScrollDistance()
            {
                float scrollingChildLength = 0;
                float scrollerLength = 0;
                if (mScrollDirection == Direction.Horizontal)
                {
                    scrollingChildLength = ContentContainer.Size.Width;
                    scrollerLength = Size.Width;
                }
                else
                {
                    scrollingChildLength = ContentContainer.Size.Height;
                    scrollerLength = Size.Height;
                }
                return Math.Max(scrollingChildLength - scrollerLength, 0);
            }

            private void ScrollBy(float displacement)
            {
                float childCurrentPosition = (mScrollDirection == Direction.Horizontal) ? ContentContainer.PositionX : ContentContainer.PositionY;
                childTargetPosition = childCurrentPosition + displacement;
                float finalTargetPosition = BoundScrollPosition(childTargetPosition);

                if (mScrollDirection == Direction.Horizontal)
                {
                    ContentContainer.PositionX = finalTargetPosition;
                }
                else
                {
                    ContentContainer.PositionY = finalTargetPosition;
                }
            }

            private float BoundScrollPosition(float targetPosition)
            {
                targetPosition = Math.Min(0, targetPosition);
                targetPosition = Math.Max(-maxScrollDistance, targetPosition);
                return targetPosition;
            }

        }

        private TapGestureDetector mItemTapDetector;
        private LongPressGestureDetector mItemLongPressDetector;

        public void Activate()
        {
            NUIApplication.SetGeometryHittestEnabled(true);
            Window window = NUIApplication.GetDefaultWindow();
            root = new View()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
            };

            var myScrollView01 = new MyScrollView()
            {
                Size2D = new Size2D(1000,1000),
            };
            myScrollView01.ContentContainer.Size2D = new Size2D(1000, 2000);
            myScrollView01.SetDirection(MyScrollView.Direction.Vertical);
            myScrollView01.ContentContainer.Layout = new LinearLayout
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                CellPadding = new Size2D(20, 20),
            };


            mItemTapDetector = new TapGestureDetector();
            mItemTapDetector.Detected += OnTapGestureDetected;

            mItemLongPressDetector = new LongPressGestureDetector();
            mItemLongPressDetector.Detected += OnLongPressGestureDetected;

            myScrollView01.Add(GetNewScrollView(Color.Orange));
            myScrollView01.Add(GetNewScrollView(Color.Blue));
            myScrollView01.Add(GetNewScrollView(Color.Red));
            myScrollView01.Add(GetNewScrollView(Color.Cyan));
            myScrollView01.Add(GetNewScrollView(Color.DarkOrange));
            myScrollView01.Add(GetNewScrollView(Color.DarkBlue));


            root.Add(myScrollView01);

            window.Add(root);
        }


        public View GetNewScrollView(Color color)
        {
            var scrollView = new View()
            {
                Size2D = new Size2D(1000, 300),
                BackgroundColor = color,
            };
            // var scrollView = new MyScrollView()
            // {
            //     Size2D = new Size2D(1000, 300),
            // };
            // scrollView.ContentContainer.Size2D = new Size2D(2000, 300);
            // scrollView.ContentContainer.BackgroundColor = color;
            // scrollView.SetDirection(MyScrollView.Direction.Horizontal);
            // scrollView.ContentContainer.Layout = new LinearLayout
            // {
            //     LinearOrientation = LinearLayout.Orientation.Horizontal,
            //     HorizontalAlignment = HorizontalAlignment.Begin,
            //     VerticalAlignment = VerticalAlignment.Center,
            //     CellPadding = new Size2D(50, 50),
            // };

            // for (int i=0; i<20; i++)
            // {
            //     var item = new View()
            //     {
            //         Size2D = new Size2D(100, 100),
            //         BackgroundColor = Color.DarkBlue,
            //     };
            //     // item.TouchEvent += (s, e) =>
            //     // {
            //     //     bool ret = mItemTapDetector.HandleEvent(s as View, e.Touch);
            //     //     ret |= mItemLongPressDetector.HandleEvent(s as View, e.Touch);
            //     //     return ret;
            //     // };
            //     scrollView.Add(item);
            // }
            return scrollView;
        }

        private void OnTapGestureDetected(object source, TapGestureDetector.DetectedEventArgs e)
        {
             Tizen.Log.Error("NUI", $"OnTapGestureDetected {e.TapGesture.State}\n");
             e.View.BackgroundColor = Color.White;
        }

        private void OnLongPressGestureDetected(object source, LongPressGestureDetector.DetectedEventArgs e)
        {
            Tizen.Log.Error("NUI", $"OnLongPressGestureDetected {e.LongPressGesture.State}\n");
            if(e.LongPressGesture.State == Gesture.StateType.Started)
            {
                e.View.BackgroundColor = Color.Yellow;
            }
            else if (e.LongPressGesture.State == Gesture.StateType.Finished || e.LongPressGesture.State == Gesture.StateType.Cancelled)
            {
                e.View.BackgroundColor = Color.DarkBlue;
            }
        }



        public void Deactivate()
        {
            NUIApplication.SetGeometryHittestEnabled(false);
            if (root != null)
            {
                NUIApplication.GetDefaultWindow().Remove(root);
                root.Dispose();
            }
        }
    }
}
