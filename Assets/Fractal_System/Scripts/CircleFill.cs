using UnityEngine;
using System;
using MathNet.Numerics;
public class CircleFill : MonoBehaviour
{

    struct Circle
    {

        public Circle(Vector2 Center, float Radius)
        {

            this.Center = Center;
            this.Radius = Radius;
        }

        public Vector2 Center;
        public float Radius;
    }

    struct Line
    {

        public Line(Vector2 A, Vector2 B)
        {

            this.A = A;
            this.B = B;
        }

        public Vector2 A;
        public Vector2 B;
    }

    private PolygonCollider2D _triangle;
    private Circle circle;

    void Awake()
    {

        _triangle = GetComponent<PolygonCollider2D>();

        circle = TriangleCircle(_triangle.points);

        CircleCollider2D cc = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;

        cc.offset = circle.Center;
        cc.radius = circle.Radius;


        Vector2[][] subTriangles = CircleSplit(_triangle.points, circle);

        for (int i = 0; i < 3; i++)
        {
            FillTriangle(subTriangles[i], circle);
            /*Circle sub = MidCircle(subTriangles[i]);

            CircleCollider2D subcc = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;

            subcc.offset = sub.Center;
            subcc.radius = sub.Radius;*/
        }
    }

    private void FillTriangle(Vector2[] points, Circle parent)
    {
        Circle circle = TriangleCircle(points);
        SpawnCircle(circle);

        SpawnCircle(EdgeCircle(new Line(points[0], points[1]), parent, circle));		
        SpawnCircle(EdgeCircle(new Line(points[2], points[0]), parent, circle));

        if (circle.Radius < 0.05f)
            return;

        FillTriangle(CirclePointTriangle(points[0], circle), circle);
    }

    private void SpawnCircle(Circle circle)
    {

        CircleCollider2D cc = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        cc.offset = circle.Center;
        cc.radius = circle.Radius;
    }

    private Circle EdgeCircle(Line line, Circle circleA, Circle circleB)
    {

        //Bend factor of circles
        float bA = 1.0f / circleA.Radius;
        float bB = 1.0f / circleB.Radius;

        //Radius given by Descartes circle theorem
        float bs = bA + bB + 2 * Mathf.Sqrt(bA * bB);
        float radius = 1.0f / bs;

        //Centre-bend products
        Complex zA = new Complex(bA * circleA.Center.x, bA * circleA.Center.y);
        Complex zB = new Complex(bB * circleB.Center.x, bB * circleB.Center.y);

        //Limit centre-bend
        Vector2 vec = line.B - line.A;
        Vector2 vecU = vec.normalized;
        Complex zC = new Complex(-vecU.y, vecU.x);

        //Solve complex Descartes circle theorem, giving two solutions
        Complex zpA = zA + zB + zC + 2.0f * (zA * zB + zB * zC + zC * zA).SquareRoot();
        Complex zpB = zA + zB + zC - 2.0f * (zA * zB + zB * zC + zC * zA).SquareRoot();

        Vector2 centerA = new Vector2((float)zpA.Real, (float)zpA.Imaginary) / bs;
		Vector2 centerB = new Vector2((float)zpB.Real, (float)zpB.Imaginary) / bs;
		
		float errA = Mathf.Abs((circleA.Center - centerA).magnitude - (radius + circleA.Radius)) +
						Mathf.Abs((circleB.Center - centerA).magnitude - (radius + circleB.Radius));
						
						
		float errB = Mathf.Abs((circleA.Center - centerB).magnitude - (radius + circleA.Radius)) +
						Mathf.Abs((circleB.Center - centerB).magnitude - (radius + circleB.Radius));
						
						
        return new Circle(errB > errA ? centerA : centerB , radius);
    }

    private Circle TriangleCircle(Vector2[] points)
    {

        //Line length
        float la = (points[0] - points[2]).magnitude;
        float lb = (points[1] - points[0]).magnitude;
        float lc = (points[2] - points[1]).magnitude;

        //Semiperimiter length
        float s = 0.5f * (la + lb + lc);

        //Circle Radius
        float radius = Mathf.Sqrt((s - la) * (s - lb) * (s - lc) / s);

        //Center
        Vector2 center = new Vector2((la * points[1].x + lb * points[2].x + lc * points[0].x) / (2 * s), (la * points[1].y + lb * points[2].y + lc * points[0].y) / (2 * s));

        return new Circle(center, radius);
    }

    private Vector2[] CirclePointTriangle(Vector2 point, Circle circle)
    {

        Vector2[] subTriangle = new Vector2[3];

        //Copy point
        subTriangle[0] = point;

        //Vector from circle center to point
        Vector2 pc = (point - circle.Center);

        //Direction vector from circle center to point
        Vector2 pcU = pc.normalized;

        //Normal vector of pcU
        Vector2 pcN = new Vector2(pcU.y, -pcU.x);

        //Shortest distance between circle edge and point
        float crpDist = pc.magnitude - circle.Radius;

        //Distance from crpDist to original triangle edges along pcN		
        float x = circle.Radius / (Mathf.Sqrt(1 + 2 * circle.Radius / crpDist));

        //Closest point from circle edge to point
        Vector2 crp = circle.Center + pcU * circle.Radius;

        subTriangle[1] = crp + pcN * x;
        subTriangle[2] = crp - pcN * x;

        return subTriangle;
    }

    private Vector2[][] CircleSplit(Vector2[] points, Circle circle)
    {

        Vector2[][] subTriangles = new Vector2[3][];

        for (int i = 0; i < 3; i++)
        {

            //Create sub triangle array and add starting point.
            subTriangles[i] = new Vector2[3];
            subTriangles[i][0] = points[i];

            Vector2 caDir = (points[i] - circle.Center).normalized;
            Debug.DrawRay(circle.Center, caDir * circle.Radius, Color.red);

            Vector2 caNorm = new Vector2(caDir.y, -caDir.x);


            float x = (points[i] - circle.Center).magnitude - circle.Radius;

            float y = circle.Radius / (Mathf.Sqrt(1 + 2 * circle.Radius / x));


            Debug.DrawRay(circle.Center + caDir * circle.Radius, caNorm * y, Color.blue);
            Debug.DrawRay(circle.Center + caDir * circle.Radius, -caNorm * y, Color.blue);

            subTriangles[i][1] = circle.Center + caDir * circle.Radius + caNorm * y;
            subTriangles[i][2] = circle.Center + caDir * circle.Radius - caNorm * y;
        }

        return subTriangles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
