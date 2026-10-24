using System.Numerics;
using Game.Core.Paths;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public sealed class RouteTest
    {
        [Test]
        public void MeasuresLengthAcrossAllSegments()
        {
            var route = new Route(new Vector3(0, 0, 0), new Vector3(3, 0, 0), new Vector3(3, 0, 4));
            Assert.That(route.Length, Is.EqualTo(7f).Within(0.001f));
        }

        [Test]
        public void PlacesPointInsideSecondSegment()
        {
            var route = new Route(new Vector3(0, 0, 0), new Vector3(3, 0, 0), new Vector3(3, 0, 4));
            var point = route.At(5f);
            Assert.That(Vector3.Distance(point, new Vector3(3, 0, 2)), Is.LessThan(0.001f));
        }

        [Test]
        public void StaysAtStartBeforeTheRouteBegins()
        {
            var route = new Route(new Vector3(1, 0, 1), new Vector3(5, 0, 1));
            Assert.That(route.At(-2f), Is.EqualTo(new Vector3(1, 0, 1)));
        }

        [Test]
        public void StaysAtFinishBeyondTheRouteEnd()
        {
            var route = new Route(new Vector3(1, 0, 1), new Vector3(5, 0, 1));
            Assert.That(route.At(100f), Is.EqualTo(new Vector3(5, 0, 1)));
        }
    }
}
