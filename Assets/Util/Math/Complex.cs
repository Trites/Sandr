using UnityEngine;
using System.Collections;

public class Complex
{

    public float real;
    public float imaginary;

    public Complex(float real, float imaginary)
    {

        this.real = real;
        this.imaginary = imaginary;
    }
	
	public Complex(Vector2 v){
		
		real = v.x;
		imaginary = v.y;
	}
	
	public Complex sqrt(){
		
		return new Complex(Mathf.Sqrt(real), Mathf.Sqrt(imaginary));
	}

    public static Complex operator +(Complex c1, Complex c2)
    {
        return new Complex(c1.real + c2.real, c1.imaginary + c2.imaginary);
    }
	
	public static Complex operator -(Complex c1, Complex c2)
    {
        return new Complex(c1.real - c2.real, c1.imaginary - c2.imaginary);
    }
	
	public static Complex operator *(Complex c1, Complex c2)
    {
        return new Complex(c1.real * c2.real + c1.imaginary * c2.imaginary, c1.real * c2.imaginary + c1.imaginary * c2.real);
    }
	
	public static Complex operator *(float f, Complex c)
    {
        return new Complex(2*c.real, 2*c.imaginary);
    }
}
