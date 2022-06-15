using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KioscoSegundoParcial
{
    public partial class Form1 : Form
    {
        public delegate void DelegadoCreado();
        public Action DelegadoActionPositivo;
        public Action DelegadoActionNegativo;  //devuelve void, recibe algo
        public Func<int, int> DelegadoFunc; //retorna algo, recibe algo
        public Predicate<int> DelegadoPredicate; //retorna bool, recibe un objeto
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        public event DelegadoCreado NoHayChocolate;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NoHayChocolate.Invoke();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (DelegadoPredicate(8))
            {
                this.button2.BackColor = Color.Red;
            }
            else
            {
                this.button2.BackColor = Color.Blue;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                MostrarMensaje(DelegadoActionPositivo);
            }
            if (this.radioButton2.Checked)
            {
                MostrarMensaje(DelegadoActionNegativo);
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            NoHayChocolate = LlamarProveedor;
            NoHayChocolate += OfrecerChicles;
            NoHayChocolate += OfrecerChupetines;
            DelegadoActionPositivo = TodoBien;
            DelegadoActionNegativo = TodoMal;
            DelegadoPredicate = MetodoQueDevuelveBool;
            DelegadoFunc = MetodoQueDevuelveElDoble;
        }

        public bool MetodoQueDevuelveBool(int numero)
        {
            if (numero == 0)
            {
                return false;
            }
            return true;
        }
        private void MostrarMensaje(Action delegadoEnviado)
        {
            delegadoEnviado();
        }
        private void LlamarProveedor()
        {
            MessageBox.Show("Tengo que comprar mas chocolates");
        }
        private void OfrecerChicles()
        {
            MessageBox.Show("Queres un chicle?");
        }
        private void OfrecerChupetines()
        {
            MessageBox.Show("Queres un chupetin?");
        }
        private void TodoBien()
        {
            this.label1.Text = "Esta todo bien";
        }
        private void TodoMal()
        {
            this.label1.Text = "Esta todo mal";
        }

        private int MetodoQueDevuelveElDoble(int numero)
        {
            return numero * 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.label2.Text = DelegadoFunc((int)this.numericUpDown1.Value).ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Task.Run(() => MetodoQueHaceAlgo(cancelTokenSource.Token));
        }

        public void MetodoQueHaceAlgo(CancellationToken token)
        {
           
                if (!token.IsCancellationRequested)
                {
                                  
                        if (this.label3.InvokeRequired)
                        {
                            this.label3.BeginInvoke((MethodInvoker)delegate ()
                            {
                                this.label3.Text = "soy una task desde otro hilo";

                            });

                        }
                        else
                        {
                            this.label3.Text = "soy una task aca";
                        }
                     
                }
                else
                {
                    //cancela
                    if (this.label3.InvokeRequired)
                    {
                        this.label3.BeginInvoke((MethodInvoker)delegate ()
                        {
                            this.label3.Text = "hola me cancelaron";

                        });

                    }
                    else
                    {
                        this.label3.Text = "hola me cancelaron";
                    }
                }

            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            cancelTokenSource.Cancel();
            this.label3.Text = "adios";
        }
    }
}
