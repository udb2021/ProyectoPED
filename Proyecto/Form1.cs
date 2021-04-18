using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;

namespace Proyecto
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        //Creamos objetos de la clase cola, tipo clientes y pedidos (almacena los objetos)
        List<Usuario> Usuarios = new List<Usuario>();
        Queue<Pedido> Pedidos = new Queue<Pedido>();

       
    class Usuario
    {
        //Datos de Usuario
        public string CodigoUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string CodigoPedido { get; set; }


        //Datos de Ticket
        public string CodigoTicket{ get; set; }

    }

    class Pedido
    {
        //Pedido
        public string NombreUsuario { get; set; }
        public string FechaPedido { get; set; }
        public string Pedido_Seleccionado { get; set; }
        public string Precio_Pedido { get; set; }
        public string Bebida { get; set; }
        public string Precio_Bebida { get; set; }
        public double Total { get; set; }


    }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
