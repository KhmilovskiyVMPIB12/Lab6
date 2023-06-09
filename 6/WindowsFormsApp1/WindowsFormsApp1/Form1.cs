using System.Windows.Forms;
using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            FunctionDelegate equation1 = x => 2 * x * x + 3 * x + 4;
            Series series1 = new Series();
            series1.ChartType = SeriesChartType.Line;
            DrawFunc(-10.0, 10.0, series1, equation1);
            chart1.Series.Add(series1);

            // Создание экземпляра делегата для уравнения SinEquation
            FunctionDelegate equation2 = x => Math.Sin(x) / x;
            Series series2 = new Series();
            series2.ChartType = SeriesChartType.Line;
            DrawFunc(-10.0, 10.0, series2, equation2);
            chart2.Series.Add(series2);

            // Создание экземпляра интегратора RectangleIntegrator
            Integrator integrator1 = new RectangleIntegrator();
            integrator1.SetStep(0.1);
            double result1 = integrator1.Integrate(equation1, -10.0, 10.0);
            string methodName1 = integrator1.GetName();

            // Создание экземпляра интегратора TrapezoidIntegrator
            Integrator integrator2 = new TrapezoidIntegrator();
            integrator2.SetStep(0.1);
            double result2 = integrator2.Integrate(equation1, -10.0, 10.0);
            string methodName2 = integrator2.GetName();

            label1.Text = methodName1;
            label2.Text = result1.ToString();

            label3.Text = methodName2;
            label4.Text = result2.ToString();

            // Отображение результатов интегрирования
            MessageBox.Show($"Интеграл ({methodName1}) = {result1}");
            MessageBox.Show($"Интеграл ({methodName2}) = {result2}");
        }
        public void DrawFunc(double x1, double x2, Series series, FunctionDelegate function)
        {
            Chart chart = new Chart();

            chart.Series.Clear();

            double step = 0.1;

            for (double x = x1; x <= x2; x += step)
            {
                double y = function(x);
                series.Points.AddXY(x, y);
            }
        }
    }

    public abstract class Equation //Базовый Класс
    {
        public abstract double Evaluate(double x); //Тут мы обьявляем функцию(абстрактную)
    }

    public class QuadraticEquation : Equation //Наследуем базовый класс
    {
        private double a, b, c;

        public QuadraticEquation(double a, double b, double c) //Это конструктор в котором мы обьявляем наши переменные
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public override double Evaluate(double x) //Тут переопределяем функцию и задаем ей действия
        {
            return a * x * x + b * x + c;
        }
    }

    public class SinEquation : Equation //Тут тоже самое только отличется  вычислениями
    {
        private double a;

        public SinEquation(double a)
        {
            this.a = a;
        }

        public override double Evaluate(double x)
        {
            if (x != 0)
                return Math.Sin(a * x) / x;
            else
                return 1; // Чтобы избежать деления на ноль
        }
    }

    public delegate double FunctionDelegate(double x);

    public abstract class Integrator //Это уже другой базовый класс
    {
        protected double h;

        public abstract double Integrate(FunctionDelegate function, double x1, double x2); //Абстрактная функция, в целом тоже самое что и класс, только функция

        public abstract string GetName();

        public void SetStep(double step)
        {
            h = step;
        }
    }

    public class RectangleIntegrator : Integrator
    {
        public override double Integrate(FunctionDelegate function, double x1, double x2)
        {
            double sum = 0.0;

            for (double x = x1; x < x2; x += h)
            {
                double y = function(x);
                sum += y * h;
            }

            return sum;
        }

        public override string GetName()
        {
            return "Метод прямоугольников";
        }
    }

    public class TrapezoidIntegrator : Integrator
    {
        public override double Integrate(FunctionDelegate function, double x1, double x2)
        {
            double sum = 0.0;

            for (double x = x1; x < x2; x += h)
            {
                double y1 = function(x);
                double y2 = function(x + h);
                sum += (y1 + y2) * h / 2;
            }

            return sum;
        }

        public override string GetName()
        {
            return "Метод трапеций";
        }
    }
}
