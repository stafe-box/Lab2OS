using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2OS
{
    public partial class Form1 : Form
    {
        protected string _way = "Buffer.txt";
        private Thread Thread1, Thread2;
        private Mutex Mutex;
        public Form1()
        {
            InitializeComponent();
            Mutex = new Mutex();
        }

        private void GenerateNewLine()
        {
            if (Mutex.WaitOne())
            {
                int num_letters = 10;
                char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890+".ToCharArray();

                // Создаем генератор случайных чисел.
                Random rand = new Random();

                // Делаем слова.
                string word = "";
                for (int j = 1; j <= num_letters; j++)
                {
                    // Выбор случайного числа от 0 до количества символов
                    // для выбора буквы из массива букв.
                    int letter_num = rand.Next(0, letters.Length - 1);
                    // Добавить слово.
                    word += letters[letter_num];
                }
                //MessageBox.Show(word);
                StreamWriter sw = File.CreateText(_way);
                sw.WriteLine(word);
                sw.Close();
                sw.Dispose();
                //Mutex.Dispose();
                Mutex.ReleaseMutex();

            }
        }

        private void ReadAndShow()
        {
            if (Mutex.WaitOne())
            {
                StreamReader sr = File.OpenText(_way);
                string line = sr.ReadToEnd();

                MessageBox.Show($"Сгенерированна строка: {line}");
                sr.Close();
                sr.Dispose();
                //Mutex.Dispose();
                Mutex.ReleaseMutex();
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            Thread1 = new Thread(GenerateNewLine);
            Thread2 = new Thread(ReadAndShow);

            Thread1.Start();
            Thread2.Start();
        }
    }
}
