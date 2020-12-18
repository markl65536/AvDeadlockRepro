using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Animators;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;

namespace AvDeadlockRepro {
    public class BusyIndicator : Control {

        private const double TH = 6;
        private const int N = 8;
        private const double R1 = 16;
        private const double R2 = 24;
        private static readonly IPen m_Pen = (new Pen(Brushes.DarkBlue, TH) {LineCap = PenLineCap.Round}).ToImmutable();
        private IClock m_Clock;

        public static readonly StyledProperty<double> AngleProperty =
            AvaloniaProperty.Register<BusyIndicator, double>(nameof(Angle));

        static BusyIndicator() {
            AffectsRender<BusyIndicator>(AngleProperty);
        }

        public double Angle {
            get => GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public override void Render(DrawingContext context) {
            var m1 = Matrix.CreateRotation(Angle * Math.PI / 180);
            var m2 = Matrix.CreateTranslation(Bounds.Size.Width / 2, Bounds.Size.Height / 2);
            using (context.PushPostTransform(m1*m2)) {
                for (var i = 0; i < N; i++) {
                    var a = i * 2 * Math.PI / N;
                    var sin = Math.Sin(a);
                    var cos = Math.Cos(a);
                    context.DrawLine(m_Pen, new Point(sin * R1, cos * R1), new Point(sin * R2, cos * R2));
                }
            }
        }
        
        private Animation CreateAnimation() {
            var animation = new Animation() {Duration = TimeSpan.FromSeconds(3), IterationCount = IterationCount.Infinite};
            
            AddKeyFrame(0.0,0.0);
            AddKeyFrame(1.0,360.0);
            
            void AddKeyFrame(double cue, double angle) {
                var kf = new KeyFrame() {Cue = new Cue(cue)};
                kf.Setters.Add(new Setter(AngleProperty, angle));
                animation.Children.Add(kf);
            }

            return animation;
        }

        public void StartSpinner() {
            var ani = CreateAnimation();
            m_Clock = new Clock();
            ani.RunAsync(this, m_Clock).ContinueWith(t => {
                Console.WriteLine("Animation stopped");
            });
        }

        public void StopSpinner() {
            m_Clock.PlayState = PlayState.Stop;
            m_Clock = null;
            Console.WriteLine("Stopped clock");
        }
    }
}