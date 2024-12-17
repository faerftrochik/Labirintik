using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class Snowflake
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Speed { get; set; }
    public int Size { get; set; }

    public Snowflake(float x, float y, float speed, int size)
    {
        X = x;
        Y = y;
        Speed = speed;
        Size = size;
    }
}

public class SnowfallScreensaver : Form
{
    private List<Snowflake> snowflakes;
    private Random random;
    private Timer timer;
    private Image background;

    public SnowfallScreensaver()
    {
        // Устанавливаем свойства окна
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.BackColor = Color.Black;
        this.DoubleBuffered = true;

        // Загрузка фонового изображения
        background = Image.FromFile("Shegpad.jpg"); // Укажите путь к фоновому изображению

        // Инициализация списка снежинок
        snowflakes = new List<Snowflake>();
        random = new Random();

        // Создаем начальные снежинки
        for (int i = 0; i < 1000; i++) // Количество снежинок
        {
            AddSnowflake();
        }

        // Настройка таймера
        timer = new Timer();
        timer.Interval = 16; // ~60 FPS
        timer.Tick += Timer_Tick;
        timer.Start();

        // Обработка клавиш
        this.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        };
    }

    private void AddSnowflake()
    {
        float x = random.Next(0, 1600);
        float y = random.Next(-this.ClientSize.Height, 0);
        float speed = (float)random.NextDouble() * 2 + 1;
        int size = random.Next(5, 10);
        snowflakes.Add(new Snowflake(x, y, speed, size));
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        MoveSnowflakes();
        this.Invalidate();
    }

    private void MoveSnowflakes()
    {
        foreach (var snowflake in snowflakes)
        {
            snowflake.Y += snowflake.Speed;

            if (snowflake.Y > this.ClientSize.Height)
            {
                // Сбрасываем снежинку наверх
                snowflake.X = random.Next(0, this.ClientSize.Width);
                snowflake.Y = random.Next(-this.ClientSize.Height, 0);
                snowflake.Speed = (float)random.NextDouble() * 2 + 1;
                snowflake.Size = random.Next(5, 10);
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // Рисуем фон
        g.DrawImage(background, 0, 0, this.ClientSize.Width, this.ClientSize.Height);

        // Рисуем снежинки
        foreach (var snowflake in snowflakes)
        {
            using (Brush brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, snowflake.X, snowflake.Y, snowflake.Size, snowflake.Size);
            }
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new SnowfallScreensaver());
    }
}
